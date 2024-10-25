using SanGuoSha.BaseClass;


namespace SanGuoSha.EquipageProc
{
    internal class WeaponProc
    {
        /// <summary>
        /// 激活`行动玩家`的武器，使得行动玩家的某些特性改变
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aData">游戏数据</param>
        public static void ActiveWeapon(CardEffect aWeaponEffect, BattlefieldBase aBattlefield)
        {
            switch (aWeaponEffect)
            {
                case CardEffect.诸葛连弩:
                    aBattlefield.ActionPlayerData.ShaNoLimitFlags.Add(aWeaponEffect);
                    break;
            }
        }

        /// <summary>
        /// 卸载武器触发的方法
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aData">游戏数据</param>
        public static void UnloadWeapon(PlayerBase aPlayer, CardEffect aWeaponEffect, BattlefieldBase aBattlefield)
        {
            switch (aWeaponEffect)
            {
                case CardEffect.诸葛连弩:
                    var apd = aBattlefield.ActionPlayerData;
                    if(apd.CurrentPlayer == aPlayer && apd.ShaNoLimitFlags.Contains(aWeaponEffect)) apd.ShaNoLimitFlags.Remove(aWeaponEffect);
                    break;
            }
        }

        /// <summary>
        /// 获取武器对象的攻击范围值
        /// </summary>
        /// <param name="aWeapon">武器效果</param>
        /// <returns>武器的攻击范围</returns>
        public static sbyte WeaponRange(CardEffect aWeapon) => Card.Weapons.GetValueOrDefault(aWeapon, (sbyte)1);

        public static int CalcShaTargetsCount(CardEffect aWeaponEffect, Card[] aCards, PlayerBase aPlayer, int aTargetsCount)
        {
            var newCount = aTargetsCount;
            switch (aWeaponEffect)
            {
                case CardEffect.方天画戟:
                    if (aCards.Length > 0 && aPlayer.HasCardsInHand(aCards) && aPlayer.Hands.Except(aCards).Count() == 0)
                        newCount += 2;
                    break;
            }
            return newCount;
        }

        /// <summary>
        /// 计算伤害量
        /// </summary>
        /// <param name="aWeaponEffect">武器效果</param>
        /// <param name="aTarget">伤害目标</param>
        /// <param name="aDamageEffect">伤害效果</param>
        /// <param name="aOldDamage">原先的伤害量</param>
        /// <returns>返回新的伤害量</returns>
        public static sbyte CalcDamage(CardEffect aWeaponEffect, PlayerBase aTarget, CardEffect aDamageEffect, sbyte aOldDamage)
        {
            var newDamage = aOldDamage;
            switch (aWeaponEffect)
            {
                case CardEffect.古锭刀:
                    if(aDamageEffect == CardEffect.杀 && aTarget != null && !aTarget.HasHand)
                    {
                        newDamage++;
                    }
                    break;
            }

            return newDamage;
        }

        /// <summary>
        /// 对方对杀出闪时候的处理方法
        /// </summary>
        /// <param name="aWeaponEffect"></param>
        /// <param name="aSource"></param>
        /// <param name="aTarget"></param>
        /// <param name="aShaEvent"></param>
        /// <param name="aBattlefield"></param>
        /// <returns></returns>
        public static bool TargetShan(CardEffect aWeaponEffect, PlayerBase aSource, PlayerBase aTarget, EventRecord aShaEvent, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            switch (aWeaponEffect)
            {
                case CardEffect.贯石斧:
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.贯石斧发动, aSource);
                        if(response.YN && aSource.HasCard)
                        {
                            using var aaSelectCards = aBattlefield.NewAsk();
                            var responseSelect = aaSelectCards.AskForCards(AskForEnum.贯石斧弃牌, aSource);
                            if(responseSelect.Cards.Length == 2)
                            {
                                collector.DropPlayerReponse(responseSelect);
                                return true;
                            }
                        }
                    }
                    break;
                case CardEffect.青龙偃月刀:
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.青龙偃月刀发动, aSource);
                        if(response.YN)
                        {
                            using var aaSha = aBattlefield.NewAsk();
                            var responseSha = aaSha.AskForCards(AskForEnum.杀, aSource);
                            if (responseSha.Effect == CardEffect.杀)
                            {
                                collector.DropPlayerReponse(responseSha);
                                var obj = Activator.CreateInstance(aBattlefield.AggressiveCards[CardEffect.杀]);
                                var iCardEffect = (obj as ICardEffectBase)!;
                                iCardEffect.Proc(aShaEvent, aBattlefield);
                            }
                        }
                    }
                    break;
            }
            return false;
        }

        public static bool EnableTargetArmor(PlayerBase aSource)
        {
            bool ret = true;
            switch (aSource.WeaponEffect)
            {
                case CardEffect.青钢剑:
                    ret = false;
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 在游戏事件层对出牌进行调整的方法
        /// </summary>
        /// <param name="aCards"></param>
        /// <param name="aSource"></param>
        /// <param name="aTarget"></param>
        /// <param name="aBattlefield"></param>
        /// <returns></returns>
        public static bool WhenShaAccpeted(Card[] aCards, PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if (aSource.WeaponEffect == CardEffect.None) return true;
            switch (aSource.WeaponEffect)
            {
                case CardEffect.雌雄双股剑:
                    if (aSource.Gender != aTarget.Gender)
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.雌雄双股剑发动, aSource);
                        if (response.YN)
                        {
                            Card? selected = null;
                            if (aTarget.HasHand)
                            {   
                                using var aaCard = aBattlefield.NewAsk();
                                response = aaCard.AskForCards(AskForEnum.雌雄双股剑弃牌, aTarget);
                                if(response.Cards.Length == 1)
                                {
                                    selected = response.Cards[0];
                                }
                            }
                            if (!aTarget.HasHand || selected == null)
                            {
                                aBattlefield.TakingCards(aSource, 1);
                            }
                            else if(aTarget.Hands.Count == 1)
                            {
                                collector.DropCards(aTarget, [selected]);
                            }
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 处理出牌的操作
        /// </summary>
        public static AskForResult OnNewAnswer(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            switch (aAnswer.WeaponEffect)
            {
                case CardEffect.丈八蛇矛:
                    var newAnswer = aAnswer.ShallowCopy();
                    if(aAnswer.AskFor == AskForEnum.杀 || aAnswer.AskFor == AskForEnum.Aggressive)
                    if (aAnswer.Effect == CardEffect.None && aAnswer.Cards.Length == 2 && aAnswer.Leader.HasCardsInHand(aAnswer.Cards))
                        newAnswer.Effect = CardEffect.杀;
                    else
                        newAnswer.Effect = CardEffect.None;
                    return newAnswer;
                default:
                    return aAnswer;
            }
        }

        /// <summary>
        /// 是否允许执行伤害
        /// </summary>
        public static bool EnableShaDamage(PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            bool enableDamage = true;
            switch (aSource.WeaponEffect)
            {
                case CardEffect.寒冰箭:
                    if (aTarget.HasCard)
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.寒冰剑发动, aSource);
                        if(response.YN)
                        {
                            using var aaSelectCards = aBattlefield.NewAsk();
                            var responseSelect = aaSelectCards.AskForCards(AskForEnum.寒冰剑弃牌, aSource, aTarget);
                            var cards = responseSelect.Cards;
                            collector.DropCards(aTarget, cards);
                            if(cards.Length > 0) enableDamage = false;
                        }
                    }
                    break;
                case CardEffect.麒麟弓:
                    if (aTarget.Jia1Ma != null || aTarget.Jian1Ma != null)
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.麒麟弓发动, aSource);
                        if(response.YN)
                        {
                            using var aaSelectCards = aBattlefield.NewAsk();
                            var responseSelect = aaSelectCards.AskForCards(AskForEnum.麒麟弓弃牌, aSource, aTarget);
                            if(responseSelect.Cards.Length == 1)
                            {
                                if(aTarget.Jia1Ma != null && responseSelect.Cards.Contains(aTarget.Jia1Ma))
                                    aTarget.UnloadJia1(aBattlefield);
                                if(aTarget.Jian1Ma != null && responseSelect.Cards.Contains(aTarget.Jian1Ma))
                                    aTarget.UnloadJian1(aBattlefield);
                                collector.DropPlayerReponse(responseSelect);
                            }
                        }
                    }
                    break;
            }
            return enableDamage;
        }

        public static Card.ElementType ShaElement(Card[] aCards, PlayerBase aSource, Card.ElementType aElement, BattlefieldBase aBattlefield)
        {
            switch (aSource.WeaponEffect)
            {
                case CardEffect.朱雀羽扇:
                    if (aCards.Length == 1 && aCards[0].Element == Card.ElementType.None)
                    {
                        using var aa = aBattlefield.NewAsk();
                        var response = aa.AskForYN(AskForEnum.朱雀羽扇发动, aSource);
                        if (response.YN) return Card.ElementType.Fire;
                    }
                    break;
            }
            return aElement;
        }
    }
}
