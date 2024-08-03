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
        /// 判定过程
        /// </summary>
        /// <param name="aChief">执行过程的武将</param>
        /// <param name="aData">全局数据</param>
        private void Judgement(ChiefBase aChief, GlobalData aData)
        {
            //清除垃圾桶,事件队列
            FreeCardBin();
            queRecoard.Clear();
            //Debuff栈退栈循环
            while (GamePlayers[aChief].Debuff.Count != 0 && !GamePlayers[aChief].Dead)
            {
                //清除事件子队列
                lstRecoard.Clear();
                //取Debuff栈元素,退栈
                Card buff = GamePlayers[aChief].Debuff.Pop();

                //加入子事件队列
                queRecoard.Enqueue(new EventRecoard(aChief, aChief, new Card[] { buff }, buff.CardEffect, string.Empty));
                lstCardBin.Add(buff);
                //判断Debuff类型
                switch (buff.CardEffect)
                {
                        //兵粮寸断 
                    case Card.Effect.BingLiangCunDuan:
                        //无懈可击的过程
                        if (WuXieProc(aChief, Card.Effect.BingLiangCunDuan)) continue;
                        //获取判定牌
                        Card judgementBLCD = popJudgementCard(aChief, buff.CardEffect);
                        //对判定牌的处理
                        if (judgementBLCD.CardSuit != Card.Suit.Club)
                            aData.Take = false;
                        else
                            aData.Take = true;
                        AsynchronousCore.SendMessage(
                            new Beaver("removedebuff", aChief.ChiefName, Card.Cards2Beaver("cards", new Card[] { buff })).ToString());
                            //new XElement("removedebuff",
                            //    new XElement("target", aChief.ChiefName),
                            //    Card.Cards2XML("cards", new Card[] { buff })
                            //));
                        DropCards(true, CardFrom.JudgementCard, string.Empty, new Card[] { judgementBLCD }, Card.Effect.None, aChief, aChief, null);
                        break;
                    //乐不思蜀 
                    case Card.Effect.LeBuSiShu:
                        //无懈可击的过程
                        if (WuXieProc(aChief, Card.Effect.LeBuSiShu)) continue;
                        //获取判定牌
                        Card judgementLBSS = popJudgementCard(aChief, buff.CardEffect);
                        //对判定牌的处理
                        if (judgementLBSS.CardSuit != Card.Suit.Heart)
                            aData.Lead = false;
                        else
                            aData.Lead = true;
                        AsynchronousCore.SendMessage(
                            new Beaver("removedebuff",aChief.ChiefName , Card.Cards2Beaver("cards" , new Card[]{buff})).ToString());
                            //new XElement("removedebuff",
                            //    new XElement("target", aChief.ChiefName),
                            //    Card.Cards2XML("cards", new Card[] { buff })
                            //));
                        DropCards(true, CardFrom.JudgementCard, string.Empty, new Card[] { judgementLBSS }, Card.Effect.None, aChief, aChief, null);
                        break;

                    //闪电
                    case Card.Effect.ShanDian:

                        //无懈可击的过程
                        if (WuXieProc(aChief, Card.Effect.ShanDian))
                        {
                            //没有费血,把debuff牌从垃圾桶里拣出来
                            PickRubbish(new Card[] { buff });
                            //闪电需要挂到下一位武将的判定区,如果下一位的判定区有闪电,那就是再下一位...直到处理到该玩家为之
                            ChiefBase next = GamePlayers.NextChief(aChief);
                            //指示闪电是否安置好了
                            bool handle = false;
                            if (next != aChief)
                                do
                                {
                                    //如果这个武将没有闪电,那就把闪电放到他的debuff中
                                    if (!GamePlayers[next].HasDebuff(Card.Effect.ShanDian))
                                    {
                                        GamePlayers[next].Debuff.Push(buff);
                                        AsynchronousCore.SendMessage(
                                            new Beaver("sd.moved", aChief.ChiefName, next.ChiefName, Card.Cards2Beaver("cards", new Card[] { buff })).ToString());
                                            //new XElement("sd.moved",
                                            //    new XElement("from", aChief.ChiefName),
                                            //    new XElement("to", next.ChiefName),
                                            //    Card.Cards2XML("cards", new Card[] { buff })
                                            //));
                                        //闪电已安置
                                        handle = true;
                                        break;
                                    }
                                } while (aChief != (next = GamePlayers.NextChief(next)));


                            //如果闪电没有安置,那就挂到玩家的新debuff栈中
                            if (!handle)
                            {
                                gData.stkNewBuff.Push(buff.GetOriginalCard());
                            }
                            continue;
                        }
                        //取出判定牌
                        Card judgementSD = popJudgementCard(aChief, buff.CardEffect);
                        //费血
                        if (judgementSD.CardSuit == Card.Suit.Spade && judgementSD.CardIndex > 1 && judgementSD.CardIndex < 10)
                        {
                            AsynchronousCore.SendMessage(
                                new Beaver("removedebuff",aChief.ChiefName , Card.Cards2Beaver("cards" , new Card[]{buff})).ToString()
                            //new XElement("removedebuff",
                            //    new XElement("target", aChief.ChiefName),
                            //    Card.Cards2XML("cards", new Card[] { buff }))
                            );
                            if (GamePlayers[aChief].Armor != null)
                                DamageHealth(aChief, Armor.CalcDamage(3, new Card[] { buff }, GamePlayers[aChief].Armor.CardEffect), null, new EventRecoard(null, aChief, null, new Card[] { buff }, Card.Effect.ShanDian, string.Empty));
                            else
                                DamageHealth(aChief, 3, null, new EventRecoard(null, aChief, null, new Card[] { buff }, Card.Effect.ShanDian, string.Empty));
                        }
                        else
                        {
                            //没有费血,把debuff牌从垃圾桶里拣出来
                            PickRubbish(new Card[] { buff });
                            //闪电需要挂到下一位武将的判定区,如果下一位的判定区有闪电,那就是再下一位...直到处理到该玩家为之
                            ChiefBase next = GamePlayers.NextChief(aChief);
                            //指示闪电是否安置好了
                            bool handle = false;
                            if (next != aChief)
                                do
                                {
                                    //如果这个武将没有闪电,那就把闪电放到他的debuff中
                                    if (!GamePlayers[next].HasDebuff(Card.Effect.ShanDian))
                                    {
                                        GamePlayers[next].Debuff.Push(buff);
                                        AsynchronousCore.SendMessage(
                                            new Beaver("sd.moved",aChief.ChiefName , next.ChiefName , Card.Cards2Beaver("cards" , new Card[]{buff})).ToString()
                                            //new XElement("sd.moved",
                                            //    new XElement("from", aChief.ChiefName),
                                            //    new XElement("to", next.ChiefName),
                                            //    Card.Cards2XML("cards", new Card[] { buff }))
                                            );
                                        //闪电已安置
                                        handle = true;
                                        break;
                                    }
                                } while (aChief != (next = GamePlayers.NextChief(next)));


                            //如果闪电没有安置,那就挂到玩家的新debuff栈中
                            if (!handle)
                            {
                                gData.stkNewBuff.Push(buff.GetOriginalCard());
                            }
                        }

                        //判定牌加入子事件节点
                        DropCards(true, CardFrom.JudgementCard, string.Empty, new Card[] { judgementSD }, Card.Effect.None, aChief, aChief, null);
                        break;

                }
                //清除子事件
                lstRecoard.Clear();
            }
            //设置玩家的debuff栈
            GamePlayers[aChief].Debuff = gData.stkNewBuff;
            //清除事件记录
            FreeCardBin();
            queRecoard.Clear();
            lstRecoard.Clear();
        }
    }
}
