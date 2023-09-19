using Helpers.Consts;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes
{
    public class OverTimeStatEffect : BaseModifyStatEffect
    {
        public override void Init(StatEffectConfig parameter, Stat statToEffectToEffect)
        {
            base.Init(parameter, statToEffectToEffect);
            
            Stats.Add((int)Constant.StatsId.Duration, new Stat("Duration", parameter.Duration, float.MaxValue, (int)Constant.StatsId.Duration));
            
            GAME_TIME.TimerHandler.StartNewTimer(Stats[(int)Constant.StatsId.Duration].CurrentValue, "OverTome statEffect timer",Dispose);
        }

        public override bool ProcessEffect(ref float statValue)
        {
            statValue = StatModifier.ProcessStatModifier(statValue);
            return true;
        }
    }
}