using System;
using System.Collections.Generic;
using Tools.Enums;
using Tzipory.ConfigFiles.VisualSystemConfig;
using Tzipory.Tools.Interface;

namespace Tzipory.ConfigFiles.VisualSystemConfig
{
    public abstract class BaseStatusEffect : IDisposable ,IInitialization<StatusEffectConfig>
    {
        #region Fields

        protected readonly Stat StatToEffect;

        protected List<StatModifier> Modifiers;
        
        protected string StatusEffectName;

        #endregion

        #region Property

        public global::Tools.Enums.EffectType EffectType { get; private set; }  
        
        public bool IsDone { get; protected set; }

        public EffectSequenceConfig EffectSequence { get; private set; }

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
            EffectType = statusEffectConfig.EffectType;
            
            StatToEffect = statToEffectToEffect;

            Modifiers = new List<StatModifier>();

            foreach (var modifier in statusEffectConfig.StatModifier)
                Modifiers.Add(new StatModifier(modifier.Modifier, modifier.StatusModifierType));
            
            IsDone = false;
        }
        
        public virtual void Init(StatusEffectConfig parameter)
        {
            StatusEffectName = parameter.StatusEffectName;
            //StatusEffectToInterrupt = parameter.StatusEffectToInterrupt;
            EffectSequence = parameter.EffectSequence;
            
            Modifiers = new List<StatModifier>();

            foreach (var modifier in parameter.StatModifier)
                Modifiers.Add(new StatModifier(modifier.Modifier, modifier.StatusModifierType));
            
            IsInitialization = true;
            
            IsDone  = false;
        }

        public StatChangeData StatusEffectStart()
        {
            float changeDelta = 0;
            
            foreach (var statModifier in Modifiers)
            {
                statModifier.ProcessStatModifier(StatToEffect);
                changeDelta += statModifier.Value;
            }
            
            return new StatChangeData(StatusEffectName,changeDelta,StatToEffect.CurrentValue,EffectType);
        }

        public virtual void StatusEffectInterrupt()
        {
        }

        public abstract bool ProcessStatusEffect(out StatChangeData statChangeData);

        public abstract void Dispose();
    }

    public enum StatusEffectType
    {
        OverTime,
        Instant,
        Interval
    }
}