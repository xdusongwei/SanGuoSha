using System.Linq;
using SGS.ServerCore.Contest.Data;
namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        private EventRecoard HorseProc(EventRecoard r)
        {
            if (!GamePlayers[r.Source].Dead && r.Cards.Count() == 1)
            {
                PickRubbish(r.Cards);
                DropCards(false, CardFrom.None, r.SkillName, r.Cards, r.Cards[0].CardEffect, r.Source, r.Source, null);
                switch (r.Effect)
                {
                    case Card.Effect.Jia1:
                        GamePlayers[r.Source].Jia1Ma = r.Cards[0];
                        break;
                    case Card.Effect.Jian1:
                        GamePlayers[r.Source].Jian1Ma = r.Cards[0];
                        break;
                }
            }
            return r;
        }

        private EventRecoard ArmorProc(EventRecoard r)
        {
            if (!GamePlayers[r.Source].Dead)
            {
                PickRubbish(r.Cards);
                DropCards(false, CardFrom.None, r.SkillName, r.Cards, r.Cards[0].CardEffect, r.Source, r.Source, null);
                GamePlayers[r.Source].LoadArmor(r.Cards[0], this);
            }
            return r;
        }

        private EventRecoard WeaponProc(EventRecoard r)
        {
            if (!GamePlayers[r.Source].Dead)
            {
                PickRubbish(r.Cards);
                DropCards(false, CardFrom.None, r.SkillName, r.Cards, r.Cards[0].CardEffect, r.Source, r.Target, null);
                GamePlayers[r.Source].LoadWeapon(r.Cards[0], this, gData);
            }
            return r;
        }
    }
}
