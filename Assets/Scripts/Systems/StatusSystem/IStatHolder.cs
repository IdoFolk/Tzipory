using System.Collections.Generic;

namespace Tzipory.EntitySystem.StatusSystem
{
    public interface IStatHolder
    {
        public Dictionary<int,Stat> Stats { get; }
    }
}