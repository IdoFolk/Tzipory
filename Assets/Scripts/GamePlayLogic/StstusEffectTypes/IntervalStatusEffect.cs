using Tzipory.BaseSystem.TimeSystem;
using UnityEngine;

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
                
            _currentInterval = _interval.CurrentValue;
            _currentDuration = _duration.CurrentValue;
        }

        public override bool ProcessStatusEffect(out StatChangeData statChangeData)
        {
            if (_currentDuration < 0)
            {
                IsDone  = true;
                //may need to return
            }

            if (_currentInterval <= 0)
            {
                float changeDelta = 0;
                
                _currentInterval = _interval.CurrentValue;
                
                foreach (var statModifier in Modifiers)
                {
                    Debug.Log("Process");
                    statModifier.ProcessStatModifier(StatToEffect);
                    changeDelta += statModifier.Value;
                }
                
                statChangeData = new StatChangeData(StatusEffectName,changeDelta,StatToEffect.CurrentValue,EffectType);
                return true;
            }
            
            _currentDuration -= GAME_TIME.GameDeltaTime;
            _currentInterval -= GAME_TIME.GameDeltaTime;
            
            statChangeData = default;
            return false;
        }

        public override void Dispose()
        {
            IsDone = true;
        }
    }
}