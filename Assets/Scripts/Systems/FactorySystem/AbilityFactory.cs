using System;
using Tzipory.AbilitiesSystem;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.AbilitiesSystem.AbilityExecuteTypes;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.Factory
{
    public class AbilityFactory
    {
        public static IAbilityCaster GetAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent,AbilityConfig abilityConfig)
        {
            switch (abilityConfig.AbilityCastType)
            {
                case AbilityCastType.Projectile:
                    return new ProjectileAbilityCaster(entityCasterTargetingComponent,abilityConfig);
                case AbilityCastType.Instant:
                    return  new InstantAbility(entityCasterTargetingComponent,abilityConfig);
                case AbilityCastType.Self:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null; //temp
        }

        public static IAbilityExecutor GetAbilityExecutor(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            switch (abilityConfig.AbilityExecuteType)
            {
                case AbilityExecuteType.AOE:
                    return  new AoeAbilityExecuter(caster,abilityConfig);
                case AbilityExecuteType.Single:
                    return new SingleAbilityExecuter(caster,abilityConfig);
                case AbilityExecuteType.Chain:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return  null;//temp
        }
    }
}