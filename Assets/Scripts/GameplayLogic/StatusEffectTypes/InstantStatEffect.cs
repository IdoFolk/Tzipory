using Tzipory.Systems.StatusSystem;

namespace Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes
{
    public class InstantStatEffect : BaseModifyStatEffect
    {
        public override bool ProcessEffect(ref float statValue)
        {
            StatToEffect.ProcessStatModifier(StatModifier,StatProcessName,PopUpTextConfig);
            Dispose();
            
            return true;
        }
    }
}