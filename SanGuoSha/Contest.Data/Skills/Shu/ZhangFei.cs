﻿using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillPaoXiao : SkillBase
    {
        public SkillPaoXiao()
            : base("咆哮", SkillEnabled.Passive , false)
        {

        }

        public override void WeaponUpdated(ChiefBase aChief, Card aWeapon, GlobalData aData)
        {
            aData.ShaNoLimit = true;
        }

        
        public override void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {
            aData.ShaNoLimit = true;
        }

        public override void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {
            aData.ShaNoLimit = false;
        }
    }
}
