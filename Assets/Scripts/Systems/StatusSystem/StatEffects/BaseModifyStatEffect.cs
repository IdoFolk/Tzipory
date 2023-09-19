using System.Collections.Generic;
using Helpers.Consts;

namespace Tzipory.EntitySystem.StatusSystem
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