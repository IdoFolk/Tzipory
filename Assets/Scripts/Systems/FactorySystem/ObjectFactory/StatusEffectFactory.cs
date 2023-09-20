using System;
using Tzipory.Systems.StatusSystem;
using Tzipory.SerializeData.StatSystemSerializeData;
using Tzipory.Systems.StatusSystem.Stats;

namespace Tzipory.Systems.FactorySystem.ObjectFactory
{
    public class StatusEffectFactory
    {
        public static BaseStatusEffect GetStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffect)
        {
            return statusEffectConfig.StatusEffectType switch
            {
                StatusEffectType.OverTime => new OverTimeStatusEffect(statusEffectConfig,statToEffect),
                StatusEffectType.Instant => new InstantStatusEffect(statusEffectConfig,statToEffect),
                StatusEffectType.Interval => new IntervalStatusEffect(statusEffectConfig,statToEffect),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}