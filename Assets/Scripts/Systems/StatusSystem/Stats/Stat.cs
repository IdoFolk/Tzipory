using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.VisualSystemConfig;
using Sirenix.OdinInspector;
using Tools.Enums;
using Tzipory.ConfigFiles.WaveSystemConfig.StatSystemSerilazeData;
using UnityEngine;

namespace Tzipory.ConfigFiles.VisualSystemConfig
{
#if true
    [Serializable]
#endif
    public class Stat
    {
        [Obsolete("Get the data version")]
        public event Action<float> OnValueChanged;
        public event Action<StatChangeData> OnValueChangedData;
        
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private float _currentValue;
        
        private List<BaseStatusEffect> _activeStatusEffects = new();

        private float _previousSetValue;
        
        public string Name => _name;
        public float CurrentValue => _currentValue;
        public int Id { get; }
        public float BaseValue { get; }
        public float MaxValue { get; private set; }
        
        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(StatConfig statConfig)
        {
            _name = statConfig.Name;
            Id = statConfig.ID;
            BaseValue = statConfig.BaseValue;
            _currentValue = BaseValue;
            MaxValue = StatLimiters.MaxStatValue;
        }
        
        public Stat(StatSerializeData statSerializeData)
        {
            _name = statSerializeData.Name;
            Id = statSerializeData.ID;
            BaseValue = statSerializeData.BaseValue;
            _currentValue = BaseValue;
            MaxValue = StatLimiters.MaxStatValue;
        }
        
        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(string name, float baseValue,float maxValue,int id)
        {
            _name = name;
            Id = id;  
            BaseValue = baseValue;
            MaxValue = StatLimiters.MaxStatValue;
            _currentValue = BaseValue;
        }
        
        public void UpdateStatusEffects()
        {
            if (_activeStatusEffects.Count == 0)
                return;

            for (int i = 0; i < _activeStatusEffects.Count; i++)
            {
                if (_activeStatusEffects[i].IsDone)
                {
                    //Need to return to the pool
                    _activeStatusEffects.RemoveAt(i);
                    continue;
                }

                if (_activeStatusEffects[i].ProcessStatusEffect(out StatChangeData statChangeData))
                    OnValueChangedData?.Invoke(statChangeData);
            }
        }
        
        public IDisposable AddStatusEffect(StatusEffectConfig statusEffectConfig)
        {
            var statusEffect = Factory.StatusEffectFactory.GetStatusEffect(statusEffectConfig,this);
            
           return AddStatusEffect(statusEffect);
        }
        
        public IDisposable AddStatusEffect(BaseStatusEffect statusEffect)
        {
            var statChange = statusEffect.StatusEffectStart();
            
            //TODO add interrupt logic
            OnValueChangedData?.Invoke(statChange);
            
            _activeStatusEffects.Add(statusEffect);
       
            return statusEffect;
        }
        
        public void ProcessStatModifier(params StatModifier[] statModifiers)
        {
            float changeDelta = 0;
            
            foreach (var statModifier in statModifiers)
            {
                statModifier.ProcessStatModifier(this);
                changeDelta += statModifier.Value;
            }
            
            StatChangeData changeData = new StatChangeData(string.Empty,changeDelta,CurrentValue,global::Tools.Enums.EffectType.Default);
            OnValueChangedData?.Invoke(changeData);
        }
        
        private void ChangeValue(float value)
        {
            _currentValue = value;
            OnValueChanged?.Invoke(_currentValue);
        }

        public void SetValue(float amount)
        {
            _previousSetValue = _currentValue;
            ChangeValue(amount);
        }
        
        public void ResetSetValue() =>
            ChangeValue(_previousSetValue);

        public void MultiplyValue(float amount)=>
            ChangeValue(_currentValue * amount);
        
        public void DivideValue(float amount)=>
            ChangeValue(_currentValue / amount);
        
        public void AddToValue(float amount)=>
            ChangeValue(_currentValue + amount);
        
        public void ReduceFromValue(float amount) =>
            ChangeValue(_currentValue - amount);
        
        public void ResetValue() =>
            ChangeValue(BaseValue);
    }
    
    public readonly struct StatChangeData
    {
        public readonly string StatusEffectName;
        public readonly float Delta;
        public readonly float NewValue;
        public readonly global::Tools.Enums.EffectType EffectType;
        
        public StatChangeData(string statusEffectName,float delta, float newValue, global::Tools.Enums.EffectType effectType)
        {
            StatusEffectName = statusEffectName;
            Delta = delta;
            NewValue = newValue;
            EffectType = effectType;
        }
    }
}