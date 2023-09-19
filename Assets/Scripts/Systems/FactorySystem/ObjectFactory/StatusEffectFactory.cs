using System;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes;
using Tzipory.SerializeData.StatSystemSerilazeData;

namespace Tzipory.Factory
{
    public class StatusEffectFactory
    {
        public static IStatEffectProcess GetStatusEffect(StatEffectConfig statEffectConfig,Stat statToEffect)
        {
            BaseStatEffect statEffect = statEffectConfig.StatEffectType switch
            {
                StatEffectType.Process => new ProcessStatEffect(),
                StatEffectType.OverTime => new OverTimeStatEffect(),
                StatEffectType.Interval => new IntervalStatEffect(),
                StatEffectType.Instant => new InstantStatEffect(),
                _ => throw new ArgumentOutOfRangeException()
            };

            statEffect.Init(statEffectConfig,statToEffect); //TODO: need to be on the requester responsibility
            
            return statEffect;
        }
    }
}