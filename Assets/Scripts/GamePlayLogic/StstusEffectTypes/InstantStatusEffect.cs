
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    internal sealed class InstantStatusEffect : BaseStatusEffect
    {
        public InstantStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect) : base(statusEffectConfig,statToEffectToEffect)
        {
        }
        

        public override bool ProcessStatusEffect(out StatChangeData statChangeData)
        {
            IsDone  = true;
            statChangeData = default;
            return false;
        }

        public override void Dispose()
        {
            foreach (var statModifier in Modifiers)
                statModifier.Undo(StatToEffect);
        }
    }
}