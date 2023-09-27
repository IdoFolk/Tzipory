using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.SerializeData.StatSystemSerializeData;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem
{
#if UNITY_EDITOR
    [Serializable]
#endif
    public class Stat
    {
        public event Action<StatChangeData> OnValueChangedData;
        
#if UNITY_EDITOR //only for debug in the editor
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private float _currentValue;
#endif
        
        private List<IStatEffectProcess> _processStatEffect = new();
        private List<IStatEffectProcess> _modifierStatEffect = new();

        private IOrderedEnumerable<IStatEffectProcess> _orderProcessStatEffect;

        private float _dynamicValue;
        private float _zeroSetModifier;
        
        public string Name { get;}

        public float CurrentValue
        {
            get
            {
                if (_zeroSetModifier == 0)
                    return _zeroSetModifier;
                
                float output = BaseValue;
                
                output += _dynamicValue;

                if (_orderProcessStatEffect is not null)
                {
                    foreach (var statEffectProcess in _orderProcessStatEffect)
                    {
                        if (!statEffectProcess.ProcessEffect(ref output))
                        {
                            //did not process
                        }
                    }
                }
                
                if (output < 0) // make sure sure that CurrentValue don't get less than 0 
                    output = 0;

                // if (output > MaxValue)
                //     output = MaxValue;
                
                return output;
            }
        }
        public int Id { get; }
        public float BaseValue { get; }
        public float MaxValue { get; private set; }
        
        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(StatConfig statConfig)
        {
            Name = statConfig.Name;
            Id = statConfig.ID;
            BaseValue = statConfig.BaseValue;
            MaxValue = StatLimiters.MaxStatValue;
            _zeroSetModifier = 1;
#if UNITY_EDITOR
            _currentValue = BaseValue;
            _name = Name;
#endif
        }
        
        public Stat(StatSerializeData statSerializeData)
        {
            Name = statSerializeData.Name;
            Id = statSerializeData.ID;
            BaseValue = statSerializeData.BaseValue;
            MaxValue = StatLimiters.MaxStatValue;
            _zeroSetModifier = 1;
#if UNITY_EDITOR
            _currentValue = BaseValue;
            _name = Name;
#endif
        }
        
        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(string name, float baseValue,float maxValue,int id)
        {
            Name = name;
            Id = id;  
            BaseValue = baseValue;
            MaxValue = StatLimiters.MaxStatValue;
            _zeroSetModifier = 1;
#if UNITY_EDITOR
            _currentValue = BaseValue;
            _name = Name;
#endif
        }
        
        public void UpdateStatusEffects()
        {
#if UNITY_EDITOR
            _currentValue = CurrentValue;
#endif
            
            if (_modifierStatEffect.Count == 0)
                return;

            for (int i = 0; i < _modifierStatEffect.Count; i++)
            {
                _modifierStatEffect[i].ProcessEffect(ref _dynamicValue);
            }
        }
        
        public IDisposable AddStatusEffect(StatEffectConfig statEffectConfig)
        {
            var statusEffect =  FactorySystem.ObjectFactory.StatusEffectFactory.GetStatusEffect(statEffectConfig,this);
            
            return AddStatusEffect(statusEffect);
        }
        
        public IDisposable AddStatusEffect(IStatEffectProcess statEffectProcess)
        {
            //TODO: need to work on the overTime logic
            if (statEffectProcess.StatEffectType is StatEffectType.Process or StatEffectType.OverTime)
            {
                //TODO: make sure Performance
                _processStatEffect.Add(statEffectProcess);
                _orderProcessStatEffect = _processStatEffect.OrderBy(x => x.StatProcessPriority);
                OnValueChangedData?.Invoke(new StatChangeData(statEffectProcess.StatProcessName,statEffectProcess.StatModifier.Modifier,CurrentValue));
            }
            else
                _modifierStatEffect.Add(statEffectProcess);

            statEffectProcess.OnDispose += RemoveStatEffect;
            
            return statEffectProcess;
        }

        private void RemoveStatEffect(int statProcessInstanceID)
        {
            //TODO: need to find the statEffect and remove it and unsubscribe to the OnDispose event 
            if (FindStatEffectProcess(_processStatEffect,statProcessInstanceID,out var processStatEffect))
            {
                _processStatEffect.Remove(processStatEffect);
                processStatEffect.OnDispose -= RemoveStatEffect;
                _orderProcessStatEffect = _processStatEffect.OrderBy(x => x.StatProcessPriority);
                OnValueChangedData?.Invoke(new StatChangeData(processStatEffect.StatProcessName,processStatEffect.StatModifier.Modifier,CurrentValue));
                return;
            }
            if (FindStatEffectProcess(_modifierStatEffect,statProcessInstanceID,out var modifyStatEffect))
            {
                modifyStatEffect.OnDispose -= RemoveStatEffect;
                _modifierStatEffect.Remove(modifyStatEffect);
            }
            //on stat removed
        }

        public void ProcessStatModifier(StatModifier statModifier,string statEffectName = null)//may not need to be param
        {
            _dynamicValue = statModifier.ProcessStatModifier(_dynamicValue);
            
            StatChangeData changeData = new StatChangeData(statEffectName,statModifier.Modifier,CurrentValue);
            OnValueChangedData?.Invoke(changeData);
        }
        
        public void RestStatDynamicValue()
        {
            _dynamicValue = 0;
            OnValueChangedData?.Invoke(new StatChangeData($"Reset Stat {Name}",0,CurrentValue));
        }
        
        /// <summary>
        /// Set a zero set modifier
        /// </summary>
        /// <param name="amount">A value 0 or 1 to set the stat to 0 or its original value</param>
        public void SetValue(int amount)
        {
            if (amount is 0 or 1)
                _zeroSetModifier = amount;
            else
                throw new Exception("Amount in stat SetValue is not equal to 0 or 1");
        }

        private void AddAndOrderProcesses(IStatEffectProcess statEffectProcess)
        {
            
        }
        
        private bool FindStatEffectProcess(List<IStatEffectProcess> sources,int statProcessInstanceID,out IStatEffectProcess statEffectProcess)
        {
            foreach (var effectProcess in sources)
            {
                if (effectProcess.StatProcessInstanceID != statProcessInstanceID) continue;
                
                statEffectProcess = effectProcess;
                return true;
            } 
            
            statEffectProcess  = null;
            return false;
        }
    }
    
    public readonly struct StatChangeData
    {
        public readonly string StatEffectName;
        public readonly float Delta;
        public readonly float NewValue;
        //public readonly EffectType EffectType;
        
        public StatChangeData(string statEffectName,float delta, float newValue)
        {
            StatEffectName = statEffectName;
            Delta = delta;
            NewValue = newValue;
        }
    }
}