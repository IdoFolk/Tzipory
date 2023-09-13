using Tzipory.BaseSystem.TimeSystem;

namespace Tzipory.GameplayLogic.StatusEffectTypes
{
    internal sealed class OverTimeStatusEffect : BaseStatusEffect
    {
        private Stat _duration;
        private float _currentDuration;
        private bool _disableDuration;
        
        public OverTimeStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect) : base(statusEffectConfig,statToEffectToEffect)
        {
            _duration = new Stat("Duration", statusEffectConfig.Duration, int.MaxValue,999);//temp need to find what to do 
            _currentDuration = _duration.CurrentValue;
            _disableDuration = statusEffectConfig.DisableDuration;
        }

        public override bool ProcessStatusEffect(out StatChangeData statChangeData)
        {
            if (_disableDuration)
            {
                statChangeData = default;
                return false;
            }
            
            _currentDuration -= GAME_TIME.GameDeltaTime;

            if (_currentDuration > 0)
            {
                statChangeData  = default;
                return false;
            }
            
            Dispose();
            
            statChangeData = new StatChangeData(StatusEffectName,0, StatToEffect.CurrentValue,EffectType);//may need to see if relvent
            return true;
        }

        public override void Dispose()
        {
            foreach (var statModifier in Modifiers)
                statModifier.Undo(StatToEffect);
            
            IsDone  = true;
        }
    }
}