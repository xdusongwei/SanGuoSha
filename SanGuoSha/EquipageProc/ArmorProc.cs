using SanGuoSha.BaseClass;


namespace SanGuoSha.EquipageProc
{
    internal class ArmorProc
    {
        /// <summary>
        /// 卸载护甲触发的方法
        /// </summary>
        /// <param name="aArmorEffect">护甲效果</param>
        /// <param name="aData">游戏数据</param>
        public static void OnUnloadAromor(CardEffect aArmorEffect, BattlefieldBase aBattlefield)
        {
            switch (aArmorEffect)
            {
                case CardEffect.白银狮子:
                    aBattlefield.RegainHealth(aBattlefield.ActionPlayerData.CurrentPlayer, 1);
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
        public static sbyte CalcDamage(sbyte aOldDamage, CardEffect aEffect, Card[] aCards, CardEffect aArmor, Card.ElementType aElement = Card.ElementType.None)
        {
            sbyte newDamage = aOldDamage;
            switch (aArmor)
            {
                case CardEffect.藤甲:
                    if (aEffect == CardEffect.杀 && aElement == Card.ElementType.Fire)
                        newDamage += 1;
                    break;
                case CardEffect.白银狮子:
                    newDamage = 1;
                    break;
            } 
            
            return newDamage;
        }

        public static bool EnableFor(CardEffect aArmor, Card[] aCards, CardEffect aEffect, Card.ElementType aElement = Card.ElementType.None, Card.CardColor aColor = Card.CardColor.Unknown)
        {
            switch (aEffect)
            {
                case CardEffect.杀:
                    if (aArmor == CardEffect.仁王盾)
                    {
                        if (aColor == Card.CardColor.Red)
                            return true;
                        else
                            return false;
                    }
                    else if (aArmor == CardEffect.藤甲)
                    {
                        if (aEffect == CardEffect.万箭齐发 || aEffect == CardEffect.南蛮入侵 || (aEffect == CardEffect.杀 && aElement == Card.ElementType.None))
                        {
                            return false;
                        }

                    }
                    break;
            }
            return true;
        }

        public static bool WhenAskingShan(PlayerBase aTarget, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            bool result = false;
            switch (aTarget.ArmorEffect)
            {
                //八卦阵
                case  CardEffect.八卦阵:
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.八卦阵发动, aTarget);
                        if (response.YN)
                        {
                            var card = collector.PopSentenceByCard(aTarget, aTarget.ArmorEffect);
                            if (card.Color == Card.CardColor.Red)
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }
            return result;
        }
    }
}
