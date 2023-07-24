using Tzipory.BaseSystem.TimeSystem;

namespace Tzipory.EntitySystem.StatusSystem
{
    internal sealed class OverTimeStatusEffect : BaseStatusEffect
    {
        private Stat _duration;
        private float _currentDuration;
        
        public OverTimeStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect) : base(statusEffectConfig,statToEffectToEffect)
        {
            _duration = new Stat("Duration", statusEffectConfig.Duration, int.MaxValue,999 );//temp need to find what to do 
            _currentDuration = _duration.CurrentValue;
        }


        public override void StatusEffectStart()
        {
            foreach (var statModifier in modifiers)
                statModifier.ProcessStatModifier(StatToEffect);
            base.StatusEffectStart();
        }

        public override void ProcessStatusEffect()
        {
            _currentDuration -= GAME_TIME.GameDeltaTime;

            if (_currentDuration < 0)
                StatusEffectFinish();
        }

        public override void Dispose()
        {
            foreach (var statModifier in modifiers)
                statModifier.Undo(StatToEffect);
        }

        protected override void StatusEffectFinish()
        {
            Dispose();
            base.StatusEffectFinish();
        }
    }
}