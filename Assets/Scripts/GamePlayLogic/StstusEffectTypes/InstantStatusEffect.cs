
namespace Tzipory.EntitySystem.StatusSystem
{
    internal sealed class InstantStatusEffect : BaseStatusEffect
    {
        public InstantStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect) : base(statusEffectConfig,statToEffectToEffect)
        {
        }

        public override void ProcessStatusEffect()
        {
            foreach (var statModifier in modifiers)
                statModifier.ProcessStatModifier(StatToEffect);

            StatusEffectFinish();
        }

        public override void Dispose()
        {
            foreach (var statModifier in modifiers)
                statModifier.Undo(StatToEffect);
        }
    }
}