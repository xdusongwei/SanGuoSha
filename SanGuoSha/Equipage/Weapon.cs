using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using SanGuoSha.Contest.Data;
using BeaverMarkupLanguage;
namespace SanGuoSha.Contest.Equipage
{
    internal class Weapon
    {
        /// <summary>
        /// 在服务实例的回应中对牌的操作
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aCards">玩家的出牌</param>
        /// <param name="aChief">出牌武将</param>
        /// <param name="aTargets">武将目标</param>
        /// <param name="aAskFor">问询内容</param>
        /// <param name="aEffect">最终定义的牌效果</param>
        /// <param name="aData">游戏数据</param>
        public static void LeadCards(Card.Effect aWeaponEffect, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.ZhangBaSheMao:
                    if (aEffect == Card.Effect.None && aCards.Count() == 2 && aData.Game.GamePlayers[aChief].HasCardsInHand(aCards))
                    {
                        aEffect = Card.Effect.Sha;
                    }
                    break;
            }
        }

        public static void CalcShaTargetsCount(Card.Effect aWeaponEffect, Card[] aCards, ChiefBase aChief, Card.Effect aEffect, GlobalData aData , ref int aTargetsCount)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.FangTianHuaJi:
                    if (aEffect == Card.Effect.Sha && aCards.Count() > 0 && aData.Game.GamePlayers[aChief].HasCardsInHand(aCards) && aData.Active == aChief)
                    {
                        foreach (Card c in aCards)
                            if (!aData.Game.GamePlayers[aChief].Hands.Contains(c))
                                return;
                        if (aData.Game.GamePlayers[aChief].Hands.Except(aCards).Count() == 0)
                        {
                            aTargetsCount += 3;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 激活出牌玩家的武器，使得出牌玩家的某些特性改变
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aData">游戏数据</param>
        public static void ActiveWeapon(Card.Effect aWeaponEffect, GlobalData aData)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.ZhuGeLianNu:
                    aData.ShaNoLimit = true;
                    break;
            }
        }

        /// <summary>
        /// 卸载武器触发的方法
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aData">游戏数据</param>
        public static void UnloadWeapon(Card.Effect aWeaponEffect, GlobalData aData)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.ZhuGeLianNu:
                    aData.ShaNoLimit = false;
                    break;
            }
        }

        /// <summary>
        /// 获取武器对象的攻击范围值
        /// </summary>
        /// <param name="aWeapon">武器效果</param>
        /// <returns>武器的攻击范围</returns>
        public static sbyte WeaponRange(Card.Effect aWeapon)
        {
            sbyte ret = 1;
            switch (aWeapon)
            {
                case Card.Effect.ZhangBaSheMao:
                    ret = 3;
                    break;
                case Card.Effect.GuanShiFu:
                    ret = 3;
                    break;
                case Card.Effect.QingLongYanYueDao:
                    ret = 3;
                    break;
                case Card.Effect.CiXiongShuangGuJian:
                    ret = 2;
                    break;
                case Card.Effect.QingGangJian:
                    ret = 2;
                    break;
                case Card.Effect.ZhuQueYuShan:
                    ret = 4;
                    break;
                case Card.Effect.FangTianHuaJi:
                    ret = 4;
                    break;
                case Card.Effect.ZhuGeLianNu:
                    ret = 1;
                    break;
                case Card.Effect.QiLinGong:
                    ret = 5;
                    break;
                case Card.Effect.GuDianDao:
                    ret = 2;
                    break;
                case Card.Effect.HanBingJian:
                    ret = 2;
                    break;
            }
            return --ret;
        }

        /// <summary>
        /// 计算伤害量
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aSource">伤害来源</param>
        /// <param name="aTarget">伤害目标</param>
        /// <param name="aDamageEffect">伤害效果</param>
        /// <param name="aOldDamage">原先的伤害量</param>
        /// <param name="aData">游戏数据</param>
        /// <returns>返回新的伤害量</returns>
        public static sbyte CalcDamage(Card.Effect aWeaponEffect, ChiefBase aSource, ChiefBase aTarget, Card.Effect aDamageEffect, sbyte aOldDamage, GlobalData aData)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.GuDianDao:
                    if(aDamageEffect == Card.Effect.Sha && aTarget != null && aData.Game.GamePlayers[aTarget].Hands.Count == 0)
                    {
                        return ++aOldDamage;
                    }
                    break;
            }

            return aOldDamage;
        }

        public static bool EnableTargetArmorWithMessage(Card.Effect aWeaponEffect, ChiefBase aSource, ChiefBase aTarget , GlobalData aData)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.QingGangJian:
                    //aData.Game.AskCore.SendMessage(new XElement("青刚剑"));
                    break;
            }
            return EnableTargetArmor(aWeaponEffect, aSource, aTarget);
        }

        public static bool EnableTargetArmor(Card.Effect aWeaponEffect, ChiefBase aSource, ChiefBase aTarget)
        {
            bool ret = true;
            switch (aWeaponEffect)
            {
                case Card.Effect.QingGangJian:
                    ret = false;
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 是否允许执行伤害
        /// </summary>
        /// <param name="aWeaponEffect">务求效果</param>
        /// <param name="aSource">伤害来源</param>
        /// <param name="aTarget">伤害目标</param>
        /// <param name="aData">游戏数据</param>
        /// <returns>返回是表示允许伤害(默认),返回false系统将不执行伤害</returns>
        public static bool EnableShaDamage(Card.Effect aWeaponEffect, ChiefBase aSource,  ChiefBase aTarget, GlobalData aData)
        {
            bool ret = true;
            if(aSource != null && aTarget != null )
                switch (aWeaponEffect)
                {
                    case Card.Effect.HanBingJian:
                        if (aData.Game.GamePlayers[aTarget].HasCard)
                        {
                            aData.Game.AsynchronousCore.SendMessage(
                                new Beaver("askfor.hbj.select", aSource.ChiefName , aTarget.ChiefName).ToString());
                                //new XElement("askfor.hbj.select",
                                //    new XElement("target", aSource.ChiefName),
                                //    new XElement("target2" , aTarget.ChiefName)
                                //    )
                                //);
                            MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForCards(aSource, MessageCore.AskForEnum.TargetCard, aTarget);
                            if (res.Effect != Card.Effect.None)
                            {
                                ret = false;
                                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeDropMessage(aSource, aTarget,res.Cards));
                                        //new XElement("drop",
                                        //    new XElement("from", aSource.ChiefName),
                                        //    new XElement("to", aTarget.ChiefName),
                                        //    Card.Cards2XML("cards", res.Cards)
                                        //    )
                                        //);
                                aData.Game.DropCards(false, GlobalEvent.CardFrom.HandAndEquipage, string.Empty, res.Cards, Card.Effect.None, aTarget, aSource, null);
                                aData.Game.GamePlayers[aSource].Hands.AddRange(res.Cards);
                                if (aData.Game.GamePlayers[aTarget].HasCard)
                                {
                                    res = aData.Game.AsynchronousCore.AskForCards(aSource, MessageCore.AskForEnum.TargetCard, aTarget);
                                    if (res.Effect == Card.Effect.None)
                                    {
                                        res = new MessageCore.AskForResult(false, null, [], [aData.Game.AutoSelect(aTarget)], Card.Effect.GuoHeChaiQiao, false, true, string.Empty);
                                    }
                                    aData.Game.AsynchronousCore.SendMessage(
                                        MessageCore.MakeDropMessage(aSource ,aTarget , res.Cards ));
                                        //new XElement("drop",
                                        //    new XElement("from", aSource.ChiefName),
                                        //    new XElement("to", aTarget.ChiefName),
                                        //    Card.Cards2XML("cards", res.Cards)
                                        //    )
                                        //);
                                    aData.Game.DropCards(false, GlobalEvent.CardFrom.HandAndEquipage, string.Empty, res.Cards, Card.Effect.None, aTarget, aSource, null);
                                    aData.Game.GamePlayers[aSource].Hands.AddRange(res.Cards);
                                }
                            }
                        }
                        break;
                    case Card.Effect.QiLinGong:
                        if (aData.Game.GamePlayers[aTarget].Jia1Ma != null || aData.Game.GamePlayers[aTarget].Jian1Ma != null)
                        {
                            aData.Game.AsynchronousCore.SendMessage(new Beaver("askfor.qlg.select" ,aSource.ChiefName , aTarget.ChiefName).ToString());
                                //new XElement("askfor.qlg.select" ,
                                //    new XElement("target", aSource.ChiefName),
                                //    new XElement("target2" , aTarget.ChiefName )
                                //    )
                                //);
                            MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForCards(aSource, MessageCore.AskForEnum.TargetHorse, aTarget);
                            if (res.Effect != Card.Effect.None)
                            {
                                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeDropMessage(aSource , aTarget , res.Cards));
                                    //new XElement("drop",
                                    //    new XElement("from", aSource.ChiefName),
                                    //    new XElement("to", aTarget.ChiefName),
                                    //    Card.Cards2XML("cards", res.Cards)
                                    //    )
                                    //);
                                aData.Game.DropCards(false, GlobalEvent.CardFrom.Equipage, string.Empty, res.Cards, Card.Effect.None, aTarget, aSource, null);
                                aData.Game.GamePlayers[aSource].Hands.AddRange(res.Cards);
                            }
                        }
                        break;
                }
            return ret;
        }

        /// <summary>
        /// 对方对杀出闪时候的处理方法
        /// </summary>
        /// <param name="aWeaponEffect"></param>
        /// <param name="aSource"></param>
        /// <param name="aTarget"></param>
        /// <param name="aData"></param>
        /// <param name="aShaEvent"></param>
        /// <returns></returns>
        public static bool TargetShan(Card.Effect aWeaponEffect, ChiefBase aSource, ChiefBase aTarget, GlobalData aData , GlobalEvent.EventRecoard aShaEvent)
        {
            string msg = null;
            MessageCore.AskForResult res = null;
            switch (aWeaponEffect)
            {
                case Card.Effect.GuanShiFu:
                    msg = new Beaver("askfor.gsf.cards", aSource.ChiefName).ToString();
                        //new XElement("askfor.gsf.cards",
                        //    new XElement("target", aSource.ChiefName)
                        //    );
                    res = aData.Game.AsynchronousCore.AskForCards(aSource, MessageCore.AskForEnum.TargetTwoCardsWithoutWeaponAndJudgement, new AskForWrapper(msg , aData.Game), aData);
                    aData.Game.AsynchronousCore.LeadingValid(aSource);
                    if (res.Effect != Card.Effect.None)
                    {
                        aData.Game.DropCards(true, GlobalEvent.CardFrom.HandAndEquipage, res.SkillName , res.Cards, Card.Effect.None, aSource, aTarget, null);
                        return true;
                    }
                    break;
                case Card.Effect.QingLongYanYueDao:
                    msg = new Beaver("askfor.qlyy.sha", aSource.ChiefName).ToString();
                        //new XElement("askfor.qlyy.sha",
                        //    new XElement("target", aSource.ChiefName)
                        //    );
                    res = aData.Game.AsynchronousCore.AskForCards(aSource, MessageCore.AskForEnum.Sha,new AskForWrapper(msg , aData.Game) , aData);
                    aData.Game.AsynchronousCore.LeadingValid(aSource);
                    if (res.Effect == Card.Effect.Sha)
                    {
                        aData.Game.DropCards(true , GlobalEvent.CardFrom.Hand , res.SkillName , res.Cards , Card.Effect.Sha , aSource  ,aTarget , null );
                        aData.Game.ShaProc(aData.Game.lstRecoard[aData.Game.lstRecoard.Count - 1]);
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// 在游戏事件层对出牌进行调整的方法
        /// </summary>
        /// <param name="aWeaponEffect"></param>
        /// <param name="aCardsEffect"></param>
        /// <param name="aCards"></param>
        /// <param name="aData"></param>
        /// <param name="aSource"></param>
        /// <param name="aTarget"></param>
        /// <returns></returns>
        public static Card[] Lead(Card.Effect aWeaponEffect ,Card.Effect aCardsEffect ,  Card[] aCards, GlobalData aData ,ChiefBase aSource , ChiefBase aTarget)
        {
            switch (aWeaponEffect)
            {
                case Card.Effect.ZhuQueYuShan:
                    if (aCardsEffect == Card.Effect.Sha && aCards.Count() == 1 && aCards[0].Element == Card.ElementType.None)
                    {
                        aData.Game.AsynchronousCore.SendMessage(
                            new Beaver("askfor.weapon.effect", aSource.ChiefName).ToString());
                            //new XElement("askfor.weapon.effect",
                            //    new XElement("target" , aSource.ChiefName)
                            //    )
                            //);
                        MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aSource);
                        if (res.YN == true)
                        {
                            aCards[0].Element = Card.ElementType.Fire;
                        }   
                    }
                    break;
                case Card.Effect.CiXiongShuangGuJian:
                    if (aCardsEffect == Card.Effect.Sha && aSource != null && aTarget != null && (aSource.Sex != aTarget.Sex ))
                    {
                        aData.Game.AsynchronousCore.SendMessage(
                            new Beaver("askfor.weapon.effect", aSource.ChiefName).ToString());
                            //new XElement("askfor.weapon.effect",
                            //    new XElement("target", aSource.ChiefName)
                            //    )
                            //);
                        MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aSource);
                        if (res.YN == true)
                        {
                            if (aData.Game.GamePlayers[aTarget].Hands.Count != 0)
                            {   
                                aData.Game.AsynchronousCore.SendMessage(
                                    new Beaver("askfor.cxsgj.select", aTarget.ChiefName).ToString());
                                    //new XElement("askfor.cxsgj.select",
                                    //    new XElement("target", aTarget.ChiefName)
                                    //    )
                                    //);
                                res = aData.Game.AsynchronousCore.AskForCards(aTarget, MessageCore.AskForEnum.TargetHand, aTarget);
                                aData.Game.AsynchronousCore.LeadingValid(aTarget);
                            }
                            if (aData.Game.GamePlayers[aTarget].Hands.Count == 0 || res.Effect == Card.Effect.None)
                            {
                                aData.Game.TakingCards(aSource, 1);
                            }
                            else
                            {
                                aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, string.Empty, res.Cards, Card.Effect.None, aTarget, aSource, null);
                                //aData.Game.RemoveHand(aTarget, res.Cards);
                            }

                        }
                    }
                    break;
            }
            return aCards;
        }
    }
}
