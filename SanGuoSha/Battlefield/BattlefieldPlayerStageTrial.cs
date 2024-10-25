using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        /// <summary>
        /// 判定过程
        /// </summary>
        /// <param name="aPlayer">执行过程的玩家</param>
        private void Trial(PlayerBase aPlayer)
        {
            using var collector = NewCollector();
            var newDebuff = aPlayer.Slots[PlayerBase.NewDebuffSlot];
            //Debuff栈退栈循环
            while (aPlayer.Debuff.Count != 0 && !aPlayer.Dead)
            {
                //取Debuff栈元素,退栈
                var debuff = aPlayer.Debuff.Peek();
                collector.DropTrialCard(aPlayer, debuff);
                if(TrialProcs.TryGetValue(debuff.TrialEffect, out Type? value))
                {
                    var obj = Activator.CreateInstance(value);
                    var iTrialEffect = (obj as ITrialProcBase)!;
                    iTrialEffect.Proc(aPlayer, debuff.GetOriginalCard(), debuff.TrialEffect, this);
                }
                else
                {
                    newDebuff.AddCards([debuff]);
                }
            }
            //设置玩家的debuff栈
            aPlayer.Debuff = new(newDebuff.Cards.Select(i => (DebuffNode)i));
            newDebuff.Reset();
            //清除事件记录
            ClearEventNodes();
        }
    }
}