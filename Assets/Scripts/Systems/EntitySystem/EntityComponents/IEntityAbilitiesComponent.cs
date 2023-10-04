using System.Collections.Generic;
using Tzipory.Systems.AbilitySystem;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityAbilitiesComponent
    {
        public AbilityHandler AbilityHandler { get; }
    }
}