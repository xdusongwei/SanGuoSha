using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 玩家出牌阶段的处理方法
        /// </summary>
        /// <param name="aResult">问询玩家产生的结果</param>
        /// <returns>如果出牌合法,那么将返回true</returns>
        protected bool LeadEvent(MessageCore.AskForResult aResult)
        {
            //对参数的检查
            if (aResult == null) return false;  //参数不可以是null
            if (aResult.Leader == null) return false;   //问询回应者不能是null
            if (aResult.Targets == null) return false;  //问询目标数组不能是null,但可以是空数组
            if (aResult.Cards == null) return false;    //问询的出牌数组不能是null,但可以是空数组
            foreach (ChiefBase c in aResult.Targets)    //每个目标对象不能是null
                if (c == null) return false;
            foreach (Card c in aResult.Cards)   //每个牌对象不能是null
                if (c == null) return false;
            //出牌无效
            if (!CheckValid(aResult, aResult.Leader)) return false;
            //将结果安置在这些临时变量中
            ChiefBase aChiefSource = aResult.Leader;
            ChiefBase[] aChiefTarget = aResult.Targets;
            Card[] aCards = aResult.Cards;
            Card.Effect aEffect = aResult.Effect;

            //清除 打牌列表
            FreeCardBin();
            //清除事件队列
            queRecoard.Clear();
            //清除子事件队列
            lstRecoard.Clear();

            //根据牌的效果确定游戏玩法
            //并将事件安置到子事件队列
            switch (aEffect)
            {
                    //技能处理过的回应,那这里就不处理他了
                case Card.Effect.Skill:
                    DropCards(true, CardFrom.Slot, string.Empty, CardsBuffer[WGFDSlotName].Cards.ToArray(), Card.Effect.None, null, null, null);
                    CardsBuffer[WGFDSlotName].Cards.Clear();
                    //释放打牌列表,将这些牌放进弃牌堆
                    FreeCardBin();
                    //清除事件队列
                    queRecoard.Clear();
                    //清除子事件队列
                    lstRecoard.Clear();
                    return true;
                //出杀
                case Card.Effect.Sha:

                    //若出杀有限制且已经没有机会杀了不能执行
                    if (!gData.ShaNoLimit && gData.KillRemain < 1) goto FAILED;
                    
                    //if (GamePlayers[aChiefSource].Weapon != null)
                    //    Weapon.ModifyProperty(GamePlayers[aChiefSource].Weapon.CardEffect, aCards, aChiefSource, aChiefTarget, MessageCore.AskForEnum.Aggressive, Card.Effect.Sha, gData);
                    //杀的目标数量高于最大值不能执行
                    if (CalcMaxShaTargets(aChiefSource, aCards) < aChiefTarget.Count()) goto FAILED;
                    //没有目标不能执行
                    if (aChiefTarget.Count() == 0) goto FAILED;

                    //遍历目标集合,如果目标有自己或者目标已死亡或者 够不到对方不能执行
                    foreach (ChiefBase c in aChiefTarget)
                    {
                        if (c.IsMe(aChiefSource) || GamePlayers[c].Dead || !WithinShaRange(aChiefSource , c)) goto FAILED;
                        bool Enable = true;
                        foreach (ASkill s in c.Skills)
                            Enable = s.EffectFeasible(aCards, aEffect, c, Enable, gData);
                        if (!Enable) goto FAILED;
                    }
                    //如果有重复的目标也不能执行
                    if (aChiefTarget.Distinct().Count() != aChiefTarget.Count()) goto FAILED;

                    //设置杀计数器
                    if (gData.KillRemain < 2)
                        gData.KillRemain = 0;
                    else
                        --gData.KillRemain;
                    //将杀事件按目标分解成一个个小事件,并装进子事件队列
                    //即每个子事件都是玩家杀单独的玩家的处理事件
                    foreach (ChiefBase c in aChiefTarget)
                    {
                        queRecoard.Enqueue(new EventRecoard(aChiefSource, c, aCards, aEffect, aResult.SkillName));
                    }
                    
                    //发送消息
                    if (aResult.PlayerLead)
                        AsynchronousCore.SendMessage(
                            new Beaver("sha", aChiefSource.ChiefName , ChiefBase.Chiefs2Beaver("to" ,aChiefTarget) , aResult.SkillName , Card.Cards2Beaver("cards" ,aCards)).ToString());
                        //new XElement("sha",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    ChiefBase.Chiefs2XML("to", aChiefTarget),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //    );
                    //通知武将技能 玩家使用了杀效果
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //决斗
                case Card.Effect.JueDou:
                    if (aChiefTarget.Count() == 1)
                    {
                        //目标不能是自己
                        if (aChiefTarget[0].IsMe(aChiefSource)) goto FAILED;
                        //对方不能死亡
                        if (GamePlayers[aChiefTarget[0]].Dead) goto FAILED;
                        bool Enable = true;
                        foreach (ASkill s in aChiefTarget[0].Skills)
                            Enable = s.EffectFeasible(aCards, aEffect, aChiefTarget[0], Enable, gData);
                        if (!Enable) goto FAILED;
                        //安置到事件子队列
                        queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefTarget[0], aCards, aEffect, aResult.SkillName));
                        //发送消息
                        AsynchronousCore.SendMessage(
                            new Beaver("jd", aChiefSource.ChiefName, aChiefTarget[0].ChiefName, aResult.SkillName, aResult.SkillName, Card.Cards2Beaver("cards", aCards)).ToString());
                            //new XElement("jd",
                            //    new XElement("from", aChiefSource.ChiefName),
                            //    new XElement("to", aChiefTarget[0].ChiefName),
                            //    new XElement("skill", aResult.SkillName),
                            //    Card.Cards2XML("cards", aCards)
                            //    )
                            //);
                        //通知武将技能 玩家使用了效果
                        foreach (ASkill s in aChiefSource.Skills)
                            s.OnUseEffect(aChiefSource, aEffect, gData);
                    }
                    else if (aChiefTarget.Count() == 2)
                    {
                        if (aChiefTarget[0] == aChiefTarget[1]) goto FAILED;
                        if (GamePlayers[aChiefTarget[0]].Dead || GamePlayers[aChiefTarget[1]].Dead) goto FAILED;
                        bool Enable = true;
                        foreach (ASkill s in aChiefTarget[1].Skills)
                            Enable = s.EffectFeasible(aCards, aEffect, aChiefTarget[1], Enable, gData);
                        if (!Enable) goto FAILED;
                        //安置到事件子队列
                        queRecoard.Enqueue(new EventRecoard(aChiefSource , aChiefTarget[0], aChiefTarget[1], aCards, aEffect, aResult.SkillName));
                        //发送消息
                        AsynchronousCore.SendMessage(
                            new Beaver("jd" , aChiefTarget[0].ChiefName , aChiefTarget[1].ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards)).ToString());
                            //new XElement("jd",
                            //    new XElement("from", aChiefTarget[0].ChiefName),
                            //    new XElement("to", aChiefTarget[1].ChiefName),
                            //    new XElement("skill", aResult.SkillName),
                            //    Card.Cards2XML("cards", aCards)
                            //    )
                            //);
                    }
                    else
                    {
                        goto FAILED;
                    }
                    break;
                //桃
                case Card.Effect.Tao:
                    //血量不能大于等于体力上限
                    if (GamePlayers[aChiefSource].Health == GamePlayers[aChiefSource].MaxHealth) goto FAILED;
                    //安置到事件子队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    //发送消息
                    AsynchronousCore.SendMessage(
                        new Beaver("tao" , aChiefSource.ChiefName , aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                    //    new XElement("tao",
                    //        new XElement("from", aChiefSource.ChiefName),
                    //        new XElement("to", aChiefSource.ChiefName),
                    //        new XElement("skill", aResult.SkillName),
                    //        Card.Cards2XML("cards", aCards)
                    //    )
                    //);
                    //通知武将技能 玩家使用了效果
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //南蛮入侵
                case Card.Effect.NanManRuQin:
                    //下面是把出牌玩家以后的其他玩家依次装入子事件列表中
                    ChiefBase t = GamePlayers.NextChief(aChiefSource);
                    while (!t.IsMe(aChiefSource))
                    {
                        queRecoard.Enqueue(new EventRecoard(aChiefSource, t, aCards, aEffect, aResult.SkillName));
                        t = GamePlayers.NextChief(t);
                    }
                    AsynchronousCore.SendMessage(
                        new Beaver("nmrq", aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" ,aCards )).ToString());
                        //new XElement("nmrq",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //万箭齐发
                case Card.Effect.WanJianQiFa:
                    //下面是把出牌玩家以后的其他玩家依次装入子事件列表中
                    ChiefBase t2 = GamePlayers.NextChief(aChiefSource);
                    while (!t2.IsMe(aChiefSource))
                    {
                        queRecoard.Enqueue(new EventRecoard(aChiefSource, t2, aCards, aEffect, aResult.SkillName));
                        t2 = GamePlayers.NextChief(t2);
                    }
                    AsynchronousCore.SendMessage(
                        new Beaver("wjqf" , aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                        //new XElement("wjqf",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //桃园结义
                case Card.Effect.TaoYuanJieYi:
                    //下面是把从出牌玩家开始的所有玩家依次装入子事件列表中
                    ChiefBase t3 = aChiefSource;
                    do
                    {
                        if (GamePlayers[t3].Health < GamePlayers[t3].MaxHealth)
                            queRecoard.Enqueue(new EventRecoard(aChiefSource, t3, aCards, aEffect, aResult.SkillName));
                        t3 = GamePlayers.NextChief(t3);
                    } while (!t3.IsMe(aChiefSource));
                    AsynchronousCore.SendMessage(
                        new Beaver("tyjy",aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                        //new XElement("tyjy",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //    );
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //无中生有
                case Card.Effect.WuZhongShengYou:
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("wzsy",aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" ,aCards)).ToString());
                        //new XElement("wzsy",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //过河拆桥
                case Card.Effect.GuoHeChaiQiao:
                    //必须有目标
                    if (aChiefTarget.Count() != 1) goto FAILED;
                    //目标不能是自己
                    if (aChiefTarget[0].IsMe(aChiefSource)) goto FAILED;
                    //对方不能死亡
                    if (GamePlayers[aChiefTarget[0]].Dead) goto FAILED;
                    //对方要有牌
                    if (!GamePlayers[aChiefTarget[0]].HasCardWithJudgementArea) goto FAILED;
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefTarget[0], aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("ghcq", aChiefSource.ChiefName , aChiefTarget[0].ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                        //new XElement("ghcq",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("to", aChiefTarget[0].ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                case Card.Effect.ShunShouQianYang:
                    //必须有目标
                    if (aChiefTarget.Count() != 1) goto FAILED;
                    //目标不能是自己
                    if (aChiefTarget[0].IsMe(aChiefSource)) goto FAILED;
                    //对方不能死亡
                    if (GamePlayers[aChiefTarget[0]].Dead) goto FAILED;
                    //对方要有牌
                    if (!GamePlayers[aChiefTarget[0]].HasCardWithJudgementArea) goto FAILED;
                    //可以够到对方
                    if (!WithinKitRange(aChiefSource, aChiefTarget[0])) goto FAILED;
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefTarget[0], aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("sspy" , aChiefSource.ChiefName , aChiefTarget[0].ChiefName,aResult.SkillName ,Card.Cards2Beaver("cards" , aCards)).ToString());
                        //new XElement("ssqy",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("to", aChiefTarget[0].ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //借刀杀人
                case Card.Effect.JieDaoShaRen:
                    //必须有两个目标
                    if (aChiefTarget.Count() != 2) goto FAILED;
                    //第一个目标不能是自己
                    if (aChiefTarget[0].IsMe(aChiefSource)) goto FAILED;
                    //第一个目标不能死亡
                    if (GamePlayers[aChiefTarget[0]].Dead) goto FAILED;
                    //第二个目标不能死亡
                    if (GamePlayers[aChiefTarget[1]].Dead) goto FAILED;
                    //两个目标不能一样
                    if (aChiefTarget[0].IsMe(aChiefTarget[1])) goto FAILED;
                    //必须能能够到对方
                    if (!WithinShaRange(aChiefTarget[0], aChiefTarget[1])) goto FAILED;
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefTarget[0], aChiefTarget[1], aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("jdsr", aChiefSource.ChiefName , aChiefTarget[0].ChiefName , aChiefTarget[1].ChiefName ,aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                    //new XElement("jdsr",
                    //    new XElement("from", aChiefSource.ChiefName),
                    //    new XElement("to", aChiefTarget[0].ChiefName),
                    //    new XElement("to2", aChiefTarget[1].ChiefName),
                    //    new XElement("skill", aResult.SkillName),
                    //    Card.Cards2XML("cards", aCards)
                    //    )
                    //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //乐不思蜀
                case Card.Effect.LeBuSiShu:
                    //必须有目标且目标不能使自己且目标不能死亡
                    if (aChiefTarget.Count() != 1 || aChiefTarget[0] == aChiefSource || GamePlayers[aChiefTarget[0]].Dead) goto FAILED;
                    //目标不能有乐不思蜀buff
                    if (GamePlayers[aChiefTarget[0]].HasDebuff(Card.Effect.LeBuSiShu)) goto FAILED;
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefTarget[0], aCards, aEffect, aResult.SkillName));
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //闪电
                case Card.Effect.ShanDian:
                    //玩家不能死亡
                    if (GamePlayers[aChiefSource].Dead) goto FAILED;
                    //玩家不能有闪电buff
                    if (GamePlayers[aChiefSource].HasDebuff(Card.Effect.ShanDian)) goto FAILED;
                    //添加进子事件队列
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //五谷丰登
                case Card.Effect.WuGuFengDeng:
                    ChiefBase t4 = aChiefSource;
                    do
                    {
                        queRecoard.Enqueue(new EventRecoard(aChiefSource, t4, aCards, aEffect, aResult.SkillName));
                        t4 = GamePlayers.NextChief(t4);
                    } while (!t4.IsMe(aChiefSource));
                    //List<Card> lstWGFD = CardsHeap.Pop(GamePlayers.PeoplealiveCount).ToList();
                    //lstWGFDBuff = lstWGFD;
                    CardsBuffer[WGFDSlotName].Cards.Clear();
                    CardsBuffer[WGFDSlotName].Cards.AddRange(CardsHeap.Pop(GamePlayers.PeoplealiveCount));
                    AsynchronousCore.SendMessage(
                        new Beaver("wgfd", aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , CardsBuffer[WGFDSlotName].Cards.ToArray())).ToString());
                        //new XElement("wgfd",
                        //new XElement("from", aChiefSource.ChiefName),
                        //new XElement("skill", aResult.SkillName),
                        //Card.Cards2XML("cards", CardsBuffer[WGFDSlotName].Cards.ToArray())));

                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //八卦阵
                case Card.Effect.BaGuaZhen:
                //藤甲
                case Card.Effect.TengJia:
                //仁王盾
                case Card.Effect.RenWangDun:
                //白银狮子
                case Card.Effect.BaiYinShiZi:
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("armor", aChiefSource.ChiefName , aResult.SkillName , Card.Cards2Beaver("cards" , aCards )).ToString());
                        //new XElement("armor",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                //武器
                case Card.Effect.ZhangBaSheMao:
                case Card.Effect.ZhuGeLianNu:
                case Card.Effect.GuDianDao:
                case Card.Effect.QiLinGong:
                case Card.Effect.GuanShiFu:
                case Card.Effect.QingLongYanYueDao:
                case Card.Effect.QingGangJian:
                case Card.Effect.CiXiongShuangGuJian:
                case Card.Effect.ZhuQueYuShan:
                case Card.Effect.FangTianHuaJi:
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("weapon" , aChiefSource.ChiefName , aResult.SkillName , aResult.Effect.ToString() , Card.Cards2Beaver("cards" , aCards)).ToString());
                        //new XElement("weapon",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    new XElement("effect", aResult.Effect),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;
                case Card.Effect.Jia1:
                case Card.Effect.Jian1:
                    queRecoard.Enqueue(new EventRecoard(aChiefSource, aChiefSource, aCards, aEffect, aResult.SkillName));
                    AsynchronousCore.SendMessage(
                        new Beaver("horse" , aChiefSource.ChiefName , aResult.SkillName , aResult.Effect.ToString() ,Card.Cards2Beaver("cards" , aCards )).ToString());
                        //new XElement("horse",
                        //    new XElement("from", aChiefSource.ChiefName),
                        //    new XElement("skill", aResult.SkillName),
                        //    new XElement("effect", aResult.Effect),
                        //    Card.Cards2XML("cards", aCards)
                        //    )
                        //);
                    foreach (ASkill s in aChiefSource.Skills)
                        s.OnUseEffect(aChiefSource, aEffect, gData);
                    break;

                default:
                    return false;
            }
            if(aResult.PlayerLead)
                if (!RemoveHand(aChiefSource, aCards)) goto FAILED;

            AsynchronousCore.LeadingValid(aChiefSource);
            //将牌放入 打牌列表
            if (aResult.PlayerLead)
                lstCardBin.AddRange(aCards);
            //处理子事件
            while (queRecoard.Count != 0)
            {
                EventProc();
                //清除子事件节点
                lstRecoard.Clear();
            }
            //删除五谷丰登牌堆中的牌
            DropCards(true, CardFrom.Slot, string.Empty, CardsBuffer[WGFDSlotName].Cards.ToArray(), Card.Effect.None, null, null, null);
            CardsBuffer[WGFDSlotName].Cards.Clear();
            //释放打牌列表,将这些牌放进弃牌堆
            FreeCardBin();
            //清除事件队列
            queRecoard.Clear();
            //清除子事件队列
            lstRecoard.Clear();


            //执行成功
            return true;
        FAILED:
            AsynchronousCore.LeadingInvalid(aChiefSource);
            DropCards(true, CardFrom.Slot, string.Empty, CardsBuffer[WGFDSlotName].Cards.ToArray(), Card.Effect.None, null, null, null);
            CardsBuffer[WGFDSlotName].Cards.Clear();
            //释放打牌列表,将这些牌放进弃牌堆
            FreeCardBin();
            //清除事件队列
            queRecoard.Clear();
            //清除子事件队列
            lstRecoard.Clear();
            //执行成功
            return false;
        }
    }
}
