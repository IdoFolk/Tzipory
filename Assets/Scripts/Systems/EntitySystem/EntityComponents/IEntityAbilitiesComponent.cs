using System.Collections.Generic;
using Tzipory.GameplayLogic.AbilitySystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityAbilitiesComponent
    {
        public AbilityHandler AbilityHandler { get; }
    }
}