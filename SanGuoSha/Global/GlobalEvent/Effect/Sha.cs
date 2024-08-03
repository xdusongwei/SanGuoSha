using SanGuoSha.Contest.Data;
using SanGuoSha.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 杀的过程
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        internal EventRecoard ShaProc(EventRecoard r)
        {
            foreach (ASkill s in r.Target.Skills)
                s.PreprocessingSubEvent(r.Target,ref r, gData);
            //这里再检查一次玩家是否死亡是怕玩家在以前的子事件中挂掉了,如果真挂了就忽略掉这次事件
            if (!GamePlayers[r.Target].Dead)
            {
                if (GamePlayers[r.Source].Weapon != null)
                    Weapon.Lead(GamePlayers[r.Source].Weapon.CardEffect, r.Effect, r.Cards, gData, r.Source, r.Target);
                if (GamePlayers[r.Target].Armor == null || (GamePlayers[r.Source].Weapon != null && Weapon.EnableTargetArmorWithMessage(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target, gData) && Armor.EnableFor(GamePlayers[r.Target].Armor.CardEffect, r.Cards, Card.Effect.Sha, r.Target)) || !(GamePlayers[r.Source].Weapon != null && Weapon.EnableTargetArmor(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target)))
                {
                    MessageCore.AskForResult res = null;
                    bool EnableDamage = false;
                    int times = 1;
                    foreach (ASkill s in r.Source.Skills)
                        times = s.CalcAskforTimes(r.Source, r.Target, Card.Effect.Sha, times, gData);
                    for (int i = 0; i < times; i++)
                    {
                        res = null;
                        EnableDamage = false;
                        string msg = new Beaver("askfor.sha.shan", r.Target.ChiefName, r.Source.ChiefName).ToString();
                            //new XElement("askfor.sha.shan",
                            //            new XElement("target", r.Target.ChiefName),
                            //            new XElement("source", r.Source.ChiefName)
                            //            );
                        if (GamePlayers[r.Target].Armor != null && (GamePlayers[r.Source].Weapon == null || Weapon.EnableTargetArmor(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target)))
                        {
                            //问询 闪
                            res = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Shan, new AskForWrapper(msg, this), gData);
                        }
                        else
                        {
                            //问询 闪
                            res = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Shan, new AskForWrapper(msg, this), false, gData);
                        }
                        //检验出牌合法性
                        ValidityResult(r.Target, ref res);

                        //问询结果放入子事件处理列表
                        if (res.PlayerLead)
                            DropCards(true, CardFrom.Hand, res.SkillName, res.Cards, res.Effect, res.Leader, r.Source, null);
                        else
                            DropCards(false, CardFrom.None, res.SkillName, res.Cards, res.Effect, res.Leader, r.Source, null);
                        
                        if (res.Effect == Card.Effect.Shan)
                        {
                            if (res.PlayerLead)
                            {
                                AsynchronousCore.LeadingValid(r.Target);
                                AsynchronousCore.SendMessage(
                                    new Beaver("sha.shan" ,r.Target.ChiefName ,r.Source.ChiefName , res.SkillName ,Card.Cards2Beaver("cards" , res.Cards )).ToString());
                                    //new XElement("sha.shan",
                                    //    new XElement("from", r.Target.ChiefName),
                                    //    new XElement("to", r.Source.ChiefName),
                                    //    new XElement("skill", res.SkillName),
                                    //    Card.Cards2XML("cards", res.Cards)
                                    //    )
                                    //);
                            }
                            foreach (ASkill s in res.Leader.Skills)
                                s.OnUseEffect(res.Leader, Card.Effect.Shan, gData);
                            if (GamePlayers[r.Source].Weapon != null)
                                EnableDamage = Weapon.TargetShan(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target, gData, r);
                        }
                        if (res.Effect == Card.Effect.None || EnableDamage)
                        {
                            break;
                        }
                    }
                    if (res == null || res.Effect == Card.Effect.None || EnableDamage)
                    {
                        sbyte cost = 1;
                        if (GamePlayers[r.Source].Weapon != null)
                            cost = Weapon.CalcDamage(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target, Card.Effect.Sha, cost, gData);
                        foreach (ASkill s in r.Source.Skills)
                            cost = s.CalcDamage(r.Source, r.Effect, cost, gData);
                        if (GamePlayers[r.Target].Armor != null && GamePlayers[r.Source].Weapon != null && Weapon.EnableTargetArmor(GamePlayers[r.Source].Weapon.CardEffect, r.Source, r.Target))
                            cost = Armor.CalcDamage(1, r.Cards, GamePlayers[r.Target].Armor.CardEffect);
                        DamageHealth(r.Target, cost, r.Source, r);
                    }
                }
            }
            return r;
        }
    }
}
