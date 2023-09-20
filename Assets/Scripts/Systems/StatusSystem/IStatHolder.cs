using System.Collections.Generic;
using Tzipory.Systems.StatusSystem.Stats;

namespace Tzipory.Systems.StatusSystem
{
    public interface IStatHolder
    {
        public Dictionary<int,Stat> Stats { get; }

        public IEnumerable<IStatHolder> GetNestedStatHolders();
    }
}