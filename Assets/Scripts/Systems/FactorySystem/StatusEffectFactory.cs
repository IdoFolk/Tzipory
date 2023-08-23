using System;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.SerializeData.StatSystemSerilazeData;

namespace Tzipory.Factory
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