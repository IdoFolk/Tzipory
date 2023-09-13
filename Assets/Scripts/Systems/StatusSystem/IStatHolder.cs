using System.Collections.Generic;

namespace Tzipory.ConfigFiles.VisualSystemConfig
{
    public interface IStatHolder
    {
        public Dictionary<int,Stat> Stats { get; }

        public IEnumerable<IStatHolder> GetNestedStatHolders();
    }
}