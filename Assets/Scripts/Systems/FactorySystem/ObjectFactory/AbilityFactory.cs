using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.AbilitySystem.AbilityExecuteTypes;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.FactorySystem.ObjectFactory
{
    public class AbilityFactory
    {
        [Obsolete("Use AbilitySerializeData")]
        public static IAbilityExecutor GetAbilityExecutor(ITargetAbleEntity caster,AbilityConfig parameter)
        {
            IAbilityExecutor secondaryAbilityExecute;
            
            if (parameter.HaveSecondaryAbilityExecuteType)
                secondaryAbilityExecute = GetSecondaryAbilityExecute(caster,parameter);
            else
            {
                var statEffectExecuter = new StatEffectExecuter();
                statEffectExecuter.Init(parameter.StatusEffectConfigs);
                secondaryAbilityExecute = statEffectExecuter;
            }
            
            switch (parameter.AbilityExecute.AbilityExecuteType)
            {
                case AbilityExecuteType.AOE:
                    var aoeExecuter = new AoeInstantiateExecuter();
                    aoeExecuter.Init(caster,parameter.AbilityExecute,secondaryAbilityExecute,parameter.AbilityVisualConfig);
                    return aoeExecuter;
                case AbilityExecuteType.StatExecuter:
                    return new StatEffectExecuter();
                case AbilityExecuteType.Chain:
                    break;
                case AbilityExecuteType.Projectile:
                    var projectileExecuter = new ProjectileInstantiateExecuter();
                    projectileExecuter.Init(caster,parameter.AbilityExecute,secondaryAbilityExecute,parameter.AbilityVisualConfig);
                    return projectileExecuter;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return null;//temp
        }
        
        private static IAbilityExecutor GetSecondaryAbilityExecute(ITargetAbleEntity caster,AbilityConfig parameter)
        {
            StatEffectExecuter statEffectExecuter = new StatEffectExecuter();
            statEffectExecuter.Init(parameter.StatusEffectConfigs);
            
            switch (parameter.SecondaryAbilityExecute.AbilityExecuteType)
            {
                case AbilityExecuteType.AOE:
                    var aoeExecuter = new AoeInstantiateExecuter();
                    aoeExecuter.Init(caster,parameter.SecondaryAbilityExecute,statEffectExecuter,parameter.AbilityVisualConfig);
                    return aoeExecuter;
                case AbilityExecuteType.StatExecuter:
                    return new StatEffectExecuter();
                case AbilityExecuteType.Chain:
                    break;
                case AbilityExecuteType.Projectile:
                    var projectileExecuter = new ProjectileInstantiateExecuter();
                    projectileExecuter.Init(caster,parameter.SecondaryAbilityExecute,statEffectExecuter,parameter.AbilityVisualConfig);
                    return projectileExecuter;
                default:
                    return null;
            }

            return null;
        }
    }
}