using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityAbilitiesComponent : IEntityComponent,IInitialization<BaseGameEntity,AbilityComponentConfig> , IStatHolder
    {
        public bool IsCasting { get; }
        public void CastAbilityByName(string abilityName, IEnumerable<ITargetAbleEntity> availableTargets);
        public void CastAbility(IEnumerable<ITargetAbleEntity> availableTargets);
        public void CancelCast();
    }
}