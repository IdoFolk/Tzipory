using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.StatusSystem
{
    public abstract class BaseStatEffect : IStatEffectProcess ,IInitialization<StatEffectConfig,Stat> , IStatHolder
    {
        public event Action<int> OnDispose;
        
        #region Property
        
        public string StatProcessName { get; private set; }
        public int StatProcessInstanceID { get; private set; }
        public int StatProcessPriority { get; private set; }
        public Stat StatToEffect { get; private set; }
        public StatModifier StatModifier { get; private set; }
        public StatEffectType StatEffectType { get; private set; }
        public Dictionary<int, Stat> Stats { get; private set; }
        protected PopUpTextConfig PopUpTextConfig  { get; private set; }
        public bool IsInitialization { get; private set; }
        
        #endregion
        
        public virtual void Init(StatEffectConfig parameter,Stat statToEffectToEffect)
        {
            StatProcessName = parameter.StatusEffectName;
            
            StatModifier = new StatModifier(parameter.StatModifier);
            
            StatToEffect = statToEffectToEffect;

            StatProcessPriority = parameter.StatProcessPriority;

            StatEffectType = parameter.StatEffectType;

            if (parameter.UsePopUpTextConfig)
                PopUpTextConfig = parameter.PopUpTextConfig;
            
            IsInitialization = true;

            Stats = new Dictionary<int, Stat>();
            
            StatProcessInstanceID = InstanceIDGenerator.GetInstanceID();
        }
        
        public abstract bool ProcessEffect(ref float statValue);

        public virtual void Dispose()
        {
            OnDispose?.Invoke(StatProcessInstanceID);
        }

        public abstract IEnumerable<IStatHolder> GetNestedStatHolders();
    }
}