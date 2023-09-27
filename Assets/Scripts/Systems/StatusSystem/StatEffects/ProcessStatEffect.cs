using System.Collections.Generic;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.EntitySystem.StatusSystem
{
    public class ProcessStatEffect : BaseStatEffect
    {
        public override bool ProcessEffect(ref float statValue)
        {
            statValue = StatModifier.ProcessStatModifier(statValue);
            return true;
        }

        public override IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
    }
}