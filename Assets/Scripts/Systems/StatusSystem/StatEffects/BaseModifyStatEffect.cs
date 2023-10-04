using System.Collections.Generic;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.Systems.StatusSystem
{
    public abstract class BaseModifyStatEffect : BaseStatEffect
    {
        public bool IsDone => Stats[(int)Constant.StatsId.Duration].CurrentValue <= 0;
        
        public override IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this  };
            return statHolders;
        }
    }
}