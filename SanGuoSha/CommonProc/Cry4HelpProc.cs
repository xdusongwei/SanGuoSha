using SanGuoSha.BaseClass;


namespace SanGuoSha.CommonProc
{
    internal partial class CommonProc
    {
        public static bool Cry4HelpProc(PlayerBase? aDamageSource, PlayerBase aPreDefunct, sbyte aRescuePoint, BattlefieldBase aBattlefield)
        {
            if (aPreDefunct == null || aRescuePoint < 1) return true;
            var start = aDamageSource ?? aPreDefunct;
            var player = start;
            sbyte remainRescuePoint = aRescuePoint;
            do
            {
                var askForType = player == aPreDefunct ? AskForEnum.桃或酒 : AskForEnum.桃;
                using var aa = aBattlefield.NewAsk();
                var r = aa.AskForCards(askForType, player);
                if ((askForType == AskForEnum.桃或酒 && (r.Effect == CardEffect.桃 || r.Effect == CardEffect.酒)) || (askForType == AskForEnum.桃 && r.Effect == CardEffect.桃))
                {
                    sbyte cost = 1;
                    foreach (var s in aPreDefunct.Skills)
                    {
                        cost = s.CalcRescuePoint(aPreDefunct, r.Leader, r.Effect, cost, aBattlefield);
                    }
                    remainRescuePoint -= cost;
                    using var collector = aBattlefield.NewCollector();
                    collector.DropPlayerReponse(r);
                    if (remainRescuePoint <= 0) break;
                    continue;
                }
            } while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, start));

            var newHealthPoint = sbyte.Max(0, (sbyte)(1 - remainRescuePoint));
            aPreDefunct.Health = newHealthPoint;
            return newHealthPoint > 0;
        }
    }
}
