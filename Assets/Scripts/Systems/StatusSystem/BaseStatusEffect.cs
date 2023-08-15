using System;
using System.Collections.Generic;
using SerializeData.VisualSystemSerializeData;
using Tzipory.Tools.Interface;

namespace Tzipory.EntitySystem.StatusSystem
{
    public abstract class BaseStatusEffect : IDisposable ,IInitialization<StatusEffectConfig>
    {
        #region Events

        public event Action<int> OnStatusEffectStart;
        public event Action<int> OnStatusEffectDone;
        public event  Action<int> OnStatusEffectInterrupt;

        #endregion

        #region Fields

        protected Stat StatToEffect;
        
        protected List<StatModifier> modifiers;

        #endregion

        #region Property

        public string StatusEffectName { get;private set; }

        public string AffectedStatName => StatToEffect.Name;
        public int AffectedStatId => StatToEffect.Id;

        public bool IsDone { get; private set; }

        public EffectSequenceConfig EffectSequence { get;private set; }

        public List<StatusEffectConfig> StatusEffectToInterrupt { get;private set; }
        public bool IsInitialization { get; private set; }

        #endregion

        protected BaseStatusEffect()
        {
            
        }
        
        [Obsolete("Need to use init to use pool")]
        protected BaseStatusEffect(StatusEffectConfig statusEffectConfig,Stat statToEffectToEffect)
        {
            StatusEffectName = statusEffectConfig.StatusEffectName;
            //StatusEffectToInterrupt = statusEffectConfig.StatusEffectToInterrupt;
            EffectSequence = statusEffectConfig.EffectSequence;

            StatToEffect = statToEffectToEffect;

            modifiers = new List<StatModifier>();

            foreach (var modifier in statusEffectConfig.StatModifier)
            {
                modifiers.Add(new StatModifier(modifier.Modifier, modifier.StatusModifierType));
            } 
        }
        
        public virtual void Init(StatusEffectConfig parameter)
        {
            StatusEffectName = parameter.StatusEffectName;
            //StatusEffectToInterrupt = parameter.StatusEffectToInterrupt;
            EffectSequence = parameter.EffectSequence;
            
            modifiers = new List<StatModifier>();

            foreach (var modifier in parameter.StatModifier)
                modifiers.Add(new StatModifier(modifier.Modifier, modifier.StatusModifierType));
            
            IsInitialization = true;
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

        public virtual void StatusEffectFinish()
        {
            OnStatusEffectDone?.Invoke(AffectedStatId);
            IsDone = true;
        }

        public abstract void ProcessStatusEffect();

        public abstract void Dispose();
    }
    
    public enum StatusEffectType
    {
        OverTime,
        Instant,
        Interval
    }
}