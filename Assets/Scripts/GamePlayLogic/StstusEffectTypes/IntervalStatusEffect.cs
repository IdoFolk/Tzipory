using Tzipory.BaseSystem.TimeSystem;

namespace Tzipory.EntitySystem.StatusSystem
{
    internal sealed class IntervalStatusEffect : BaseStatusEffect
    {
        private readonly Stat _interval;
        private readonly Stat _duration;
        
        private float _currentInterval;
        private float _currentDuration;
        
        public IntervalStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect) : base(statusEffectConfig,statToEffectToEffect)
        {
            _interval = new Stat("Interval", statusEffectConfig.Interval, int.MaxValue, 999);
            _duration = new Stat("Duration", statusEffectConfig.Duration, int.MaxValue, 999);
        }

        public override void StatusEffectStart()
        {
            _currentInterval = _interval.CurrentValue;
            _currentDuration = _duration.CurrentValue;
            
            foreach (var statModifier in modifiers)
                statModifier.ProcessStatModifier(StatToEffect);
            
            base.StatusEffectStart();
        }

        public override void ProcessStatusEffect()
        {
            _currentDuration -= GAME_TIME.GameDeltaTime;

            if (_currentDuration < 0)
            {
                StatusEffectFinish();
                return;
            }

            _currentInterval -= GAME_TIME.GameDeltaTime;
            if (_currentInterval <= 0)
            {
                _currentInterval = _interval.CurrentValue;
                foreach (var statModifier in modifiers)
                {
#if UNITY_EDITOR
                   // Debug.Log($"Cast effect {StatToEffect.Name} by {statModifier.Modifier.CurrentValue}");    
#endif
                    statModifier.ProcessStatModifier(StatToEffect);
                }
            }
        }

        public override void Dispose()
        {
            StatusEffectFinish();
        }
    }
}