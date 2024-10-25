using System.Text;
using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.Battlefield
{
    /// <summary>
    /// 玩家
    /// </summary>
    public partial class Player: PlayerBase
    {
        public override IEnumerable<SkillBase> Skills
        {
            get
            {
                if(Chief != null){
                    return Chief.Skills.AsEnumerable();
                }
                return [];
            }
        }

        public override ChiefBase.GenderType Gender
        {
            get
            {
                return Chief.Gender;
            }
        }

        internal override bool RemoveHand(Card aCard)
        {
            if (aCard == null) return false;
            foreach (var c in Hands)
            {
                if (aCard.IsSame(c))
                {
                    Hands.Remove(c);
                    return true;
                }
            }
            return false;
        }

        internal override bool RemoveDebuff(Card aCard)
        {
            if (aCard == null) return false;
            Stack<DebuffNode> tmp = new();
            bool ret = false;
            if(Debuff.Count != 0 && aCard.IsSame(Debuff.Peek()))
            {
                Debuff.Pop();
                return true;
            }
            while (Debuff.Count != 0)
            {
                if (aCard.IsSame(Debuff.Peek()))
                {
                    Debuff.Pop();
                    ret = true;
                }
                else
                    tmp.Push(Debuff.Pop());
            }
            while (tmp.Count != 0)
            {
                Debuff.Push(tmp.Pop());
            }
            return ret;
        }

        internal override void LoadJian1(Card aHorse, BattlefieldBase aBattlefield)
        {
            if(aHorse.CardEffect != CardEffect.减1马) throw new EquipageLoadError(aHorse);
            using var collector = aBattlefield.NewCollector();
            if (Jian1Ma != null)
                collector.EquipageReplace(this, Jian1Ma);
            Jian1Ma = aHorse;
        }

        internal override void UnloadJian1(BattlefieldBase aBattlefield)
        {
            if (Jian1Ma != null)
            {
                Jian1Ma = null;
            }
        }

        internal override void LoadJia1(Card aHorse, BattlefieldBase aBattlefield)
        {
            if(aHorse.CardEffect != CardEffect.加1马) throw new EquipageLoadError(aHorse);
            using var collector = aBattlefield.NewCollector();
            if (Jia1Ma != null)
                collector.EquipageReplace(this, Jia1Ma);
            Jia1Ma = aHorse;
        }

        internal override void UnloadJia1(BattlefieldBase aBattlefield)
        {
            if (Jia1Ma != null)
            {
                Jia1Ma = null;
            }
        }

        internal override void LoadWeapon(Card aWeapon, BattlefieldBase aBattlefield)
        {
            if(!Card.Weapons.ContainsKey(aWeapon.CardEffect)) throw new EquipageLoadError(aWeapon);
            using var collector = aBattlefield.NewCollector();
            if (WeaponEffect != CardEffect.None)
                collector.EquipageReplace(this, Weapon);
            Weapon = aWeapon;
            WeaponProc.ActiveWeapon(Weapon.CardEffect, aBattlefield);
            foreach (var s in Skills)
                s.WeaponUpdated(this, Weapon ,aBattlefield);
        }

        internal override void UnloadWeapon(BattlefieldBase aBattlefield)
        {
            if (WeaponEffect != CardEffect.None)
            {
                WeaponProc.UnloadWeapon(this, WeaponEffect, aBattlefield);
                Weapon = null;
                foreach (var s in Skills)
                    s.WeaponUpdated(this, Weapon, aBattlefield);
            }
        }

        internal override void LoadArmor(Card aArmor, BattlefieldBase aBattlefield)
        {
            if(!Card.Armors.Contains(aArmor.CardEffect)) throw new EquipageLoadError(aArmor);
            using var collector = aBattlefield.NewCollector();
            if (ArmorEffect != CardEffect.None)
                collector.EquipageReplace(this, Armor);
            Armor = aArmor;
        }

        internal override void UnloadArmor(BattlefieldBase aBattlefield)
        {
            if (ArmorEffect != CardEffect.None)
            {
                ArmorProc.OnUnloadAromor(ArmorEffect, aBattlefield);
                Armor = null;
            }
        }

        public override void PushTrialCard(Card aCard, CardEffect aTrialEffect)
        {
            if(!Card.IsDelayKit(aTrialEffect)) throw new TrialPushError(aCard);
            var debuffNode = new DebuffNode(aCard, aTrialEffect);
            Debuff.Push(debuffNode);
        }

        internal override int CalcMaxShaTargets(Card[] aCards, BattlefieldBase aBattlefield)
        {
            int count = 1;
            if (Weapon != null)
                count = WeaponProc.CalcShaTargetsCount(Weapon.CardEffect, aCards, this, count);
            return count;
        }

        private void SlotsInit()
        {
            Slots.Slots.Add(new CardSlot(NewDebuffSlot, true, true));
        }

        /// <summary>
        /// 构造玩家
        /// </summary>
        public Player(string aUID)
        {
            UID = aUID;
            PlayerName = aUID;
            Chief = new ChiefBase.BlankChief();
            SlotsInit();
        }
    }
}
