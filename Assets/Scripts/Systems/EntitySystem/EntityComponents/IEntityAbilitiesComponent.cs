using System.Collections.Generic;
using Tzipory.Systems.AbilitySystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityAbilitiesComponent
    {
        public AbilityHandler AbilityHandler { get; }
    }
}