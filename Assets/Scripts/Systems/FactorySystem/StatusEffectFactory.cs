using System;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.Factory
{
    public class StatusEffectFactory
    {
        public static BaseStatusEffect GetStatusEffect(StatusEffectConfigSo statusEffectConfigSo,Stat statToEffect)
        {
            return statusEffectConfigSo.StatusEffectType switch
            {
                StatusEffectType.OverTime => new OverTimeStatusEffect(statusEffectConfigSo,statToEffect),
                StatusEffectType.Instant => new InstantStatusEffect(statusEffectConfigSo,statToEffect),
                StatusEffectType.Interval => new IntervalStatusEffect(statusEffectConfigSo,statToEffect),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

    }
}