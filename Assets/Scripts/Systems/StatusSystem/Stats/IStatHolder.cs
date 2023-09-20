using System.Collections.Generic;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.Systems.StatusSystem
{
    public interface IStatHolder
    {
        public Dictionary<int,Stat> Stats { get; }

        public IEnumerable<IStatHolder> GetNestedStatHolders();
    }
}