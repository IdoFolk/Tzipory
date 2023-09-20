using System;
using Tzipory.Systems.StatusSystem;
using Tzipory.SerializeData.StatSystemSerializeData;
using Tzipory.Systems.StatusSystem.Stats;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes;
using Tzipory.SerializeData.StatSystemSerilazeData;

namespace Tzipory.Systems.FactorySystem.ObjectFactory
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