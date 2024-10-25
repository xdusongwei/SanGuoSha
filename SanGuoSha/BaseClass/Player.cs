

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 玩家的定义
    /// </summary>
    public abstract class PlayerBase
    {
        public const string NewDebuffSlot = "NewDebuff";
        
        public bool IsOffline
        {
            get;
            set;
        }

        public bool IsEscaped
        {
            get;
            set;
        }

        public bool IsDeputed
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public string UID = string.Empty;

        /// <summary>
        /// 玩家的名称
        /// </summary>
        public string PlayerName = string.Empty;

        /// <summary>
        /// 玩家武将的血量
        /// </summary>
        private sbyte _Health;

        /// <summary>
        /// 玩家的血量
        /// </summary>
        public sbyte Health
        {
            get
            {
                return _Health;
            }

            set
            {
                if (value > MaxHealth)
                    _Health = MaxHealth;
                else if (value < 0)
                    _Health = 0;
                else
                    _Health = value;
            }
        }

        /// <summary>
        /// 玩家的最大血量
        /// </summary>
        public sbyte MaxHealth
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家是否受伤
        /// </summary>
        public bool Injured
        {
            get => !Dead && MaxHealth > Health;
        }

        /// <summary>
        /// 玩家武将是否死亡
        /// </summary>
        public bool Dead
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家判定区的牌
        /// </summary>
        public Stack<DebuffNode> Debuff
        {
            get;
            set;
        } = [];

        /// <summary>
        /// 玩家的武将
        /// </summary>
        public ChiefBase Chief
        {
            get;
            set;
        } = new ChiefBase.BlankChief();

        /// <summary>
        /// 玩家的手牌
        /// </summary>
        public List<Card> Hands
        {
            get;
            set;
        } = [];

        /// <summary>
        /// 玩家武将的武器
        /// </summary>
        public Card? Weapon
        {
            get;
            set;
        }

        public CardEffect WeaponEffect
        {
            get
            {
                return Weapon == null ? CardEffect.None : Weapon.CardEffect;
            }
        }

        /// <summary>
        /// 玩家武将的护甲
        /// </summary>
        public Card? Armor
        {
            get;
            set;
        }

        public CardEffect ArmorEffect
        {
            get
            {
                return Armor == null ? CardEffect.None : Armor.CardEffect;
            }
        }

        /// <summary>
        /// 玩家武将的+1马
        /// </summary>
        public Card? Jia1Ma
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将的-1马
        /// </summary>
        public Card? Jian1Ma
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家是否被铁索连环
        /// </summary>
        public bool HorizontalSet
        {
            set;
            get;
        } = false;

        /// <summary>
        /// 玩家是否被翻面
        /// </summary>
        public bool TurnSet
        {
            set;
            get;
        } = false;

        /// <summary>
        /// 判断玩家是否有牌,计算范围包括手牌,装备和判定区
        /// </summary>
        public bool HasCardWithTrialZone
        {
            get
            {
                if (Hands.Count > 0) return true;
                else if (Debuff.Count > 0) return true;
                else if (Weapon != null) return true;
                else if (Armor != null) return true;
                else if (Jia1Ma != null) return true;
                else if (Jian1Ma != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// 玩家是否有手牌
        /// </summary>
        public bool HasHand
        {
            get
            {
                if (Hands.Count > 0) return true;
                return false;
            }
        }

        /// <summary>
        /// 玩家是否有牌,计算范围包括手牌和装备
        /// </summary>
        public bool HasCard
        {
            get
            {
                if (Hands.Count > 0) return true;
                else if (Weapon != null) return true;
                else if (Armor != null) return true;
                else if (Jia1Ma != null) return true;
                else if (Jian1Ma != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// 确定这些牌都在玩家的手牌中
        /// </summary>
        /// <param name="aCards">牌数组</param>
        /// <returns>若牌数组的牌都在手牌中,则返回true</returns>
        public bool HasCardsInHand(Card[] aCards)
        {
            foreach (var c in aCards)
                if (!Hands.Contains(c)) return false;
            return true;
        }

        /// <summary>
        /// 用户检测用户的判定区是否有该类型的Debuff
        /// </summary>
        /// <param name="aEffect">要查找的效果</param>
        /// <returns></returns>
        internal bool HasDebuff(CardEffect aEffect)
        {
            foreach (var c in Debuff)
            {
                if (c.CardEffect == aEffect)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 确定这些牌都来自玩家的手牌或者装备中
        /// </summary>
        /// <param name="aCards">需要检查的牌</param>
        /// <returns>若都在手牌或者装备中，返回true</returns>
        public bool HasCardsInHandOrEquipage(Card[] aCards)
        {
            foreach (var c in aCards)
            {
                if (c == null) return false;
                if (Hands.Contains(c)) continue;
                if (c == Weapon) continue;
                if (c == Armor) continue;
                if (c == Jia1Ma) continue;
                if (c == Jian1Ma) continue;
                return false;
            }
            return true;
        }

        public abstract ChiefBase.GenderType Gender
        {
            get;
        }

        public abstract IEnumerable<SkillBase> Skills
        {
            get;
        }

        /// <summary>
        /// 牌槽
        /// </summary>
        public CardSlotContainer Slots
        {
            get;
            private set;
        } = new();

        /// <summary>
        /// 移除一张手牌
        /// </summary>
        /// <param name="aCard">要移除的手牌对象</param>
        /// <returns>移除成功返回true, 失败或者参数为null返回false</returns>
        internal abstract bool RemoveHand(Card aCard);

        /// <summary>
        /// 移除一个在判定区的牌
        /// </summary>
        /// <param name="aCard">牌对象</param>
        /// <returns>移除成功返回true , 失败或者参数为null返回false</returns>
        internal abstract bool RemoveDebuff(Card aCard);

        /// <summary>
        /// 安装-1马的方法
        /// </summary>
        /// <param name="aHorse">需要安装的牌</param>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void LoadJian1(Card aHorse, BattlefieldBase aBattlefield);

        /// <summary>
        /// 卸掉-1马的方法
        /// </summary>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void UnloadJian1(BattlefieldBase aBattlefield);

        /// <summary>
        /// 安装+1马的方法
        /// </summary>
        /// <param name="aHorse">被安装的牌</param>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void LoadJia1(Card aHorse, BattlefieldBase aBattlefield);

        /// <summary>
        /// 卸掉+1马的事件
        /// </summary>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void UnloadJia1(BattlefieldBase aBattlefield);

        /// <summary>
        /// 安装武器的方法
        /// </summary>
        /// <param name="aWeapon">被安装的牌</param>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void LoadWeapon(Card aWeapon, BattlefieldBase aBattlefield);

        /// <summary>
        /// 卸掉武器的方法
        /// </summary>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void UnloadWeapon(BattlefieldBase aBattlefield);

        /// <summary>
        /// 安装防具的方法
        /// </summary>
        /// <param name="aArmor">被安装的牌</param>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void LoadArmor(Card aArmor, BattlefieldBase aBattlefield);

        /// <summary>
        /// 卸掉防具的方法
        /// </summary>
        /// <param name="aBattlefield">游戏对象</param>
        internal abstract void UnloadArmor(BattlefieldBase aBattlefield);

        public abstract void PushTrialCard(Card aCard, CardEffect aTrialEffect);

        internal abstract int CalcMaxShaTargets(Card[] aCards, BattlefieldBase aBattlefield);

        /// <summary>
        /// 玩家的身份
        /// </summary>
        public PlayerRole Role
        {
            get;
            set;
        } = PlayerRole.Insurgent;

        /// <summary>
        /// 让玩家自动选择要弃的牌
        /// </summary>
        /// <returns>返回牌数组</returns>
        public abstract Card[] AutoAbandonment();

        /// <summary>
        /// 自动从牌槽中选择一张牌
        /// </summary>
        /// <param name="aSlot"></param>
        /// <returns></returns>
        public abstract Card? AutoSelectSlot(CardSlot aSlot);

        public override string ToString()
        {
            return $"Player:{PlayerName}";
        }
    }
}
