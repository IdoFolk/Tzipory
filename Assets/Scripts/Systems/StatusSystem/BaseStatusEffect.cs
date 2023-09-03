using System;
using System.Collections.Generic;
using SerializeData.VisualSystemSerializeData;

namespace Tzipory.EntitySystem.StatusSystem
{
    public abstract class BaseStatusEffect : IDisposable
    {
        #region Events

        public event Action<int> OnStatusEffectStart;
        public event Action<int> OnStatusEffectDone;
        public event  Action<int> OnStatusEffectInterrupt;

        #endregion

        #region Fields

        protected readonly Stat StatToEffect;
        
        protected readonly List<StatModifier> modifiers;

        #endregion

        #region Property

        public string StatusEffectName { get; }

        public string AffectedStatName => StatToEffect.Name;
        public int AffectedStatId => StatToEffect.Id;

        public bool IsDone { get; private set; }

        public EffectSequenceConfig EffectSequence { get; }

        public List<StatusEffectConfig> StatusEffectToInterrupt { get; }

        #endregion
       
        
        protected BaseStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect)
        {
            StatusEffectName = statusEffectConfig.StatusEffectName;
            StatusEffectToInterrupt = statusEffectConfig.StatusEffectToInterrupt;
            EffectSequence = statusEffectConfig.EffectSequence;

            StatToEffect = statToEffectToEffect;

            modifiers = new List<StatModifier>();

            foreach (var modifier in statusEffectConfig.StatModifier)
            {
                modifiers.Add(new StatModifier(modifier.Modifier, modifier.StatusModifierType));
            } 
        }

        public virtual void StatusEffectStart()
        {
            OnStatusEffectStart?.Invoke(AffectedStatId);
            IsDone  = false;
        }

        public virtual void StatusEffectInterrupt()
        {
            OnStatusEffectInterrupt?.Invoke(AffectedStatId);
        }

        protected virtual void StatusEffectFinish()
        {
            OnStatusEffectDone?.Invoke(AffectedStatId);
            IsDone = true;
        }

        public abstract void ProcessStatusEffect();

        public abstract void Dispose();

        //Temp?
        public virtual void SetMyFirstModifier(float newValue)
        {
            modifiers[0].SetValue(newValue);
        }
    }
    
    public enum StatusEffectType
    {
        OverTime,
        Instant,
        Interval
    }
}