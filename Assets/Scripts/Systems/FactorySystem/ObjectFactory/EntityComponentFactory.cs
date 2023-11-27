using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent;
using Tzipory.Systems.Entity;
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
                case MovementComponentType.Ground:
                    throw new NotImplementedException();
                case MovementComponentType.Air:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public class AbilityComponentFactory
    {
        public static IEntityAbilitiesComponent GetAbilitiesComponent(AbilityComponentConfig abilityComponentConfig)
        {
            IEntityMovementComponent output;
            
            throw new NotImplementedException();
        }
    }
    
    public class CombatComponentFactory
    {
        public static IEntityCombatComponent GetCombatComponent(CombatComponentConfig combatComponentConfig)
        {
            IEntityMovementComponent output;
            
            throw new NotImplementedException();
        }
    }
}