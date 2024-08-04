using System;
using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using SanGuoSha.Contest.Data;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Equipage
{
    /// <summary>
    /// 护甲方法集合
    /// </summary>
    internal class Armor
    {
        /// <summary>
        /// 卸载护甲触发的方法
        /// </summary>
        /// <param name="aArmorEffect">护甲效果</param>
        /// <param name="aData">游戏数据</param>
        public static void OnUnloadAromor(Card.Effect aArmorEffect, GlobalData aData)
        {
            switch (aArmorEffect)
            {
                case Card.Effect.BaiYinShiZi:
                    //aData.Game.AskCore.SendMessage(
                    //    new XElement("trigger",
                    //        new XElement("effect", Card.Effect.BaiYinShiZi)
                    //        )
                    //        );
                    aData.Game.RegainHealth(aData.Active, 1);
                    break;
            }
        }

        /// <summary>
        /// 计算伤害量
        /// </summary>
        /// <param name="aOldDamage">原先的伤害量</param>
        /// <param name="aCards">造成伤害的牌</param>
        /// <param name="aArmor">护甲效果</param>
        /// <returns>新的伤害量</returns>
        public static sbyte CalcDamage(sbyte aOldDamage ,  Card[] aCards , Card.Effect aArmor)
        {
            switch (aArmor)
            {
                case Card.Effect.TengJia:
                    if (aCards.Count() == 1)
                    {
                        switch (aCards[0].Element)
                        {
                            case  Card.ElementType.Fire:
                                return ++aOldDamage ;
                        }
                    }
                    break;
                case Card.Effect.BaiYinShiZi:
                    aOldDamage = 1;
                    break;
            } 
            
            return aOldDamage;
        }

        public static bool EnableFor(Card.Effect aArmor ,  Card[] aCards, Card.Effect aEffect , ChiefBase aOwner)
        {
            switch (aEffect)
            {
                
                case Card.Effect.Sha:
                    if (aArmor == Card.Effect.RenWangDun)
                    {
                        if (aCards.Where(c=>c.CardSuit == Card.Suit.Diamond || c.CardSuit == Card.Suit.Heart).Count() < 1 )
                        {
                                return false;
                        }
                    }
                    else if (aArmor == Card.Effect.TengJia)
                    {
                        if (aEffect == Card.Effect.WanJianQiFa || aEffect == Card.Effect.NanManRuQin || (aCards.Count() == 1 && aCards[0].Element == Card.ElementType.None))
                        {
                            return false;
                        }

                    }
                    break;
            }
            return true;
        }

        public static MessageCore.AskForResult AskFor(Card.Effect aAskFor ,  Card.Effect aCardEffect, ChiefBase aOwner , GlobalData aData)
        {
            switch (aCardEffect)
            {
                //八卦阵
                case  Card.Effect.BaGuaZhen:
                    if (aAskFor == Card.Effect.Shan)
                    {
                        aData.Game.AsynchronousCore.SendMessage(
                            new Beaver("askfor.armor.effect", aOwner.ChiefName).ToString()
                            //new XElement("askfor.armor.effect",
                            //    new XElement("target" , aOwner.ChiefName ))
                            );
                        MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aOwner);
                        if (res.YN == true)
                        {
                            Card ret = aData.Game.popJudgementCard(aOwner, Card.Effect.BaGuaZhen);
                            aData.Game.DropCards(true, GlobalEvent.CardFrom.JudgementCard , res.SkillName , [ret], Card.Effect.None, aOwner, null, null);
                            if (ret.CardSuit == Card.Suit.Diamond || ret.CardSuit == Card.Suit.Heart)
                            {
                                return new MessageCore.AskForResult(false, aOwner, [], [ret], Card.Effect.Shan, false, false, string.Empty);
                            }
                            else
                            {
                                return new MessageCore.AskForResult(false, aOwner, [], [ret], Card.Effect.None, false, false, string.Empty);
                            }
                        }
                    }
                    break;
            }
            return new MessageCore.AskForResult(false, aOwner, [], [], Card.Effect.None, false, false, string.Empty);
        }
    }
}
