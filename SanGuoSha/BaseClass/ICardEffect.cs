

namespace SanGuoSha.BaseClass
{
    public interface ICardEffectBase
    {
        /// <summary>
        /// 校验出牌参数的方法
        /// </summary>
        /// <exception cref="EffectPrepareError">检查失败的异常</exception>
        void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield);

        EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield);
    }
}
