using System.Collections.Generic;

namespace Tzipory.GameplayLogic.StatusEffectTypes
{
    public interface IStatHolder
    {
        public Dictionary<int,Stat> Stats { get; }

        public IEnumerable<IStatHolder> GetNestedStatHolders();
    }
}