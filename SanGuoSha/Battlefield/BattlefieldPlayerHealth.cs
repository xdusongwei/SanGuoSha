using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        internal override void RegainHealth(PlayerBase aPlayer, sbyte aRegain)
        {
            if (aPlayer.MaxHealth > aPlayer.Health)
            {
                CreateActionNode(new ActionNode(
                    aLeader: aPlayer,
                    aAction: "RegainHealth",
                    aValue: aRegain
                ));
                aPlayer.Health += aRegain;
            }
        }

        private void BuryTheDead(PlayerBase aPlayer)
        {
            List<Card> lstDrop = [.. aPlayer.Hands, .. aPlayer.Debuff];
            foreach(var slot in aPlayer.Slots)
                if(slot.Recyclable)
                {
                    lstDrop.AddRange(slot.Cards);
                    slot.Reset();
                }
            if (aPlayer.Weapon != null)
                lstDrop.Add(aPlayer.Weapon);
            if (aPlayer.Armor != null)
                lstDrop.Add(aPlayer.Armor);
            if (aPlayer.Jia1Ma != null)
                lstDrop.Add(aPlayer.Jia1Ma);
            if (aPlayer.Jian1Ma != null)
                lstDrop.Add(aPlayer.Jian1Ma);
            using var collector = NewCollector();
            collector.DropCards(aPlayer, [.. lstDrop]);
            aPlayer.Health = 0;
        }

        internal override void DamageHealth(PlayerBase aPlayer, sbyte aDamage , PlayerBase? aSource, EventRecord aSourceEvent, Card.ElementType aElement = Card.ElementType.None)
        {
            CreateActionNode(new ActionNode(
                aLeader: aPlayer,
                aAction: "DamageHealth",
                aValue: aDamage
            ));
            if (aPlayer.Health - aDamage < 1)
            {
                if (!Cry4HelpProc(aSource, aPlayer, (sbyte)Math.Abs(aPlayer.Health - aDamage - 1)))
                {
                    aPlayer.Health = 0;
                    aPlayer.Dead = true;
                    foreach(var s in aPlayer.Skills)
                        s.OnPlayerDead(aPlayer, this);
                    BuryTheDead(aPlayer);
                    RefereeProc();
                    if (aPlayer.Role == PlayerRole.Insurgent && aSource != null && !aSource.Dead)
                    {
                        TakingCards(aSource, 3);
                    }
                    return;
                }
            }
            else
            {
                aPlayer.Health -= aDamage;
            }
            foreach (var s in aPlayer.Skills)
            {
                s.OnPlayerHarmed(aSourceEvent, aSource, aPlayer, this, aDamage);
            }
        }
    }
}
