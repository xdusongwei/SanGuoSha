using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 创建子事件的新节点以及将出牌者的牌移除并处理(或不处理)到垃圾桶中
        /// </summary>
        /// <param name="aSendToBin">是否将牌放置到垃圾桶中</param>
        /// <param name="aType">牌的来源区域,以便做检查</param>
        /// <param name="aSkillName">所发动的技能名称</param>
        /// <param name="aCards">牌</param>
        /// <param name="aEffect">牌的效果</param>
        /// <param name="aSource">出牌者</param>
        /// <param name="aTargetA">目标1</param>
        /// <param name="aTargetB">目标2</param>
        /// <returns>如果牌都在aType指定的区域将返回true</returns>
        internal bool DropCards(bool aSendToBin, CardFrom aType, string aSkillName, Card[] aCards, Card.Effect aEffect, ChiefBase aSource, ChiefBase aTargetA, ChiefBase aTargetB)
        {
            switch (aType)
            {
                case CardFrom.Slot:
                    if (aSendToBin)
                    {
                        lstCardBin.AddRange(aCards);
                    }
                    break;
                case CardFrom.JudgementCard:
                    if (aSendToBin)
                    {
                        foreach (Card c in aCards)
                            RecoveryJudgementCard(c, aSource, aEffect);
                    }
                    break;
                case CardFrom.None:
                    if (aSendToBin) lstCardBin.AddRange(aCards);
                    break;
                case CardFrom.Hand:
                    if (!RemoveHand(aSource, aCards))
                    {
                        return false;
                    }
                    if (aSendToBin)
                    {
                        lstCardBin.AddRange(aCards);
                        
                    }
                    foreach (ASkill s in aSource.Skills)
                        s.OnRemoveCards(aSource, gData);
                    break;
                case CardFrom.HandAndEquipageAndJudgement:
                    if (!RemoveCard(aSource, aCards))
                    {
                        return false;
                    }
                    if (aSendToBin)
                    {
                        lstCardBin.AddRange(aCards);
                        
                    }
                    foreach (ASkill s in aSource.Skills)
                        s.OnRemoveCards(aSource, gData);
                    break;
                case CardFrom.HandAndEquipage:
                    foreach (Card c in aCards)
                        if (!GamePlayers[aSource].Hands.Contains(c))
                            if (GamePlayers[aSource].Weapon != c)
                                if (GamePlayers[aSource].Armor != c)
                                    if (GamePlayers[aSource].Jia1Ma != c)
                                        if (GamePlayers[aSource].Jian1Ma != c)
                                            return false;
                    RemoveCard(aSource, aCards);
                    if (aSendToBin)
                    {
                        lstCardBin.AddRange(aCards);
                        
                    }
                    foreach (ASkill s in aSource.Skills)
                        s.OnRemoveCards(aSource, gData);
                    break;
                case CardFrom.Equipage:
                    foreach (Card c in aCards)
                        if (GamePlayers[aSource].Weapon != c)
                            if (GamePlayers[aSource].Armor != c)
                                if (GamePlayers[aSource].Jia1Ma != c)
                                    if (GamePlayers[aSource].Jian1Ma != c)
                                        return false;
                    if (!RemoveCard(aSource, aCards))
                    {
                        return false;
                    }
                    if (aSendToBin)
                    {
                        lstCardBin.AddRange(aCards);
                        
                    }
                    foreach (ASkill s in aSource.Skills)
                        s.OnRemoveCards(aSource, gData);
                    break;
            }
            lstCardBin = lstCardBin.Distinct().ToList();
            lstRecoard.Add(new EventRecoard(aSource, aTargetA, aTargetB, aCards, aEffect, aSkillName));
            return true;
        }
    }
}
