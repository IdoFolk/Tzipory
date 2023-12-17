using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem.AIComponent;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent.MovementComponents;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.FactorySystem
{
    public class HealthComponentFactory
    {
        public static IEntityHealthComponent GetHealthComponent(HealthComponentConfig healthComponentConfig)
        {
            IEntityHealthComponent output;
            
            switch (healthComponentConfig.HealthComponentType)
            {
                case HealthComponentType.Regen:
                    throw new NotImplementedException();
                case HealthComponentType.InvincibleTime:
                    throw new NotImplementedException();
                case HealthComponentType.Standard:
                    output = new StandardHealthComponent();
                    return output;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class MovementComponentFactory
    {
        public static IEntityMovementComponent GetMovementComponent(MovementComponentConfig movementComponentConfig)
        {
            IEntityMovementComponent output;

            switch (movementComponentConfig.MovementComponentType)
            {
                case MovementComponentType.GroundMoveOnSelect:
                    return new OnSelectMoveComponent();
                case MovementComponentType.GroundMoveOnPath:
                    return new MovementOnPathComponent();
                case MovementComponentType.Air:
                    throw  new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class AbilityComponentFactory
    {
        public static IEntityAbilitiesComponent GetAbilitiesComponent(AbilityComponentConfig abilityComponentConfig)
        {
            return new AbilityHandler();
        }
    }
    
    public class CombatComponentFactory
    {
        public static IEntityCombatComponent GetCombatComponent(CombatComponentConfig combatComponentConfig)
        {
            switch (combatComponentConfig.CombatComponentType)
            {
                case CombatComponentType.Range:
                    return new SimpleRangeCombatComponent();
                case CombatComponentType.Melee:
                    return new SimpleMeleeCombatComponent();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class AIComponentFactory
    {
        public static IEntityAIComponent GetAIComponent(AIComponentConfig aiComponentConfig)
        {
            switch (aiComponentConfig.AIType)
            {
                case AIComponentType.Hero:
                    return new SimpleHeroAI();
                case AIComponentType.Enemy:
                    return  new SimpleEnemyAI();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public class AnimatorComponentFactory
    {
        public static IEntityAnimatorComponent GetAIComponent(AnimatorComponentConfig animatorComponentConfig)
        {
            switch (animatorComponentConfig.AnimatorType)
            {
                case AnimatorComponentType.Hero:
                    return new HeroAnimator();
                case AnimatorComponentType.Enemy:
                    return  new BasicEnemyAnimator();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}