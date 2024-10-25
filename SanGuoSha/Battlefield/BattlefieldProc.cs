using SanGuoSha.BaseClass;
using SanGuoSha.CommonProc;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        /// <summary>
        /// 子事件处理方法
        /// </summary>
        private void EventProc()
        {
            if (EventNodeSize() < 1) return;
            //取子事件队列的事件放置到子事件处理列表中
            var r = PopEventNode();

            //根据首事件的性质开始处理
            if(r.Effect == CardEffect.None) //...None效果一般是指,某些技能已经处理过了,也不想让这个事件走一般过程,于是有了None来跳过子事件的处理
            {
                
            }
            else if(AggressiveCards.TryGetValue(r.Effect, out Type? value))
            {
                var obj = Activator.CreateInstance(value);
                var iCardEffect = (obj as ICardEffectBase)!;
                iCardEffect.Proc(r, this);
            }
            else
            {
                throw new EffectWithoutProcError(r.Effect);
            }
        }

        internal override bool WuXieProc(PlayerBase aTarget, CardEffect aEffect, EventRecord aEvent)
        => CommonProc.CommonProc.WuXieProc(aTarget, aEffect, aEvent.IgnoreWuXie, this);

        internal override bool WuXieProc(PlayerBase aTarget, CardEffect aEffect)
        => CommonProc.CommonProc.WuXieProc(aTarget, aEffect, false, this);

        /// <summary>
        /// 角色求救过程
        /// </summary>
        /// <param name="aDamageSource">伤害的来源,可以是null</param>
        /// <param name="aPreDefunct">求救武将</param>
        /// <param name="aRescuePoint">求救血量</param>
        /// <returns>返回true表示该角色求救失败,false表示求救成功</returns>
        private bool Cry4HelpProc(PlayerBase? aDamageSource, PlayerBase aPreDefunct, sbyte aRescuePoint) 
        => CommonProc.CommonProc.Cry4HelpProc(aDamageSource, aPreDefunct, aRescuePoint, this);

        private void RefereeProc() 
        => CommonProc.CommonProc.RefereeProc(this);

        /// <summary>
        /// 清除事件链的过程
        /// </summary>
        /// <remarks>该方法是将公共牌槽,武将可回收牌槽等剩余的牌放入打牌堆中,并将发送事件结束的XML消息
        /// 清除事件链发生在每个游戏阶段的起始事件完成后</remarks>
        private void ClearSlotProc()
        {
            using var collector = NewCollector();
            //尝试回收游戏对象中牌槽容器的牌
            foreach(var slot in Slots)
                collector.DropSlotCards(slot);
            //尝试回收玩家牌槽容器中的牌
            foreach (var p in Players)
                foreach (var slot in p.Slots)
                    collector.DropSlotCards(slot);
        }
    }
}
