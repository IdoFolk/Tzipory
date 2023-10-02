using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes
{
    public class InstantStatEffect : BaseModifyStatEffect
    {
        public override bool ProcessEffect(ref float statValue)
        {
            StatToEffect.ProcessStatModifier(StatModifier);
            Dispose();
            
            return true;
        }
    }
}