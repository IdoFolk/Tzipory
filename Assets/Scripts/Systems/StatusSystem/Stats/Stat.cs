using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal.Filters;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.StatSystemSerializeData;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem
{
#if UNITY_EDITOR
    [Serializable]
#endif
    public class Stat
    {
        #region Events
        public event Action<StatChangeData> OnValueChanged;
        
        #endregion

        #region Fields

#if UNITY_EDITOR //only for debug in the editor
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private float _currentValue;
#endif
        
        private List<IStatEffectProcess> _processStatEffect = new();
        private List<IStatEffectProcess> _modifierStatEffect = new();

        private IOrderedEnumerable<IStatEffectProcess> _orderProcessStatEffect;

        private float _dynamicValue;
        private float _zeroSetModifier;

        #endregion
        
        #region Properties

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

        public float DynamicValue => _dynamicValue;

        #endregion

        #region Constructors

        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(StatConfig statConfig,StatSerializeData statSerializeData)
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

        public Stat(Constant.StatsId statsId,float value)
        {
            Name = statsId.ToString();
            Id = (int)statsId;
            BaseValue = value;
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

        #endregion

        #region PublicMethod

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
        
        public IStatEffectProcess AddStatusEffect(IStatEffectProcess statEffectProcess,PopUpTextConfig popUpTextConfig)=>
            AddStatusEffect(statEffectProcess, true,popUpTextConfig);
        
        public IStatEffectProcess AddStatusEffect(IStatEffectProcess statEffectProcess)=>
            AddStatusEffect(statEffectProcess, false);

        private IStatEffectProcess AddStatusEffect(IStatEffectProcess statEffectProcess, bool usePopUpText,
            PopUpTextConfig popUpTextConfig = default)
        {
            //TODO: need to work on the overTime logic
            if (statEffectProcess.StatEffectType is StatEffectType.Process or StatEffectType.OverTime)
            {
                //TODO: make sure Performance
                _processStatEffect.Add(statEffectProcess);
                _orderProcessStatEffect = _processStatEffect.OrderBy(x => x.StatProcessPriority);

                var delta = CurrentValue - statEffectProcess.StatModifier.ProcessStatModifier(CurrentValue);
                
                OnValueChanged?.Invoke(usePopUpText
                    ? new StatChangeData(statEffectProcess.StatProcessName,delta,statEffectProcess.StatModifier.Modifier,
                        CurrentValue, popUpTextConfig)
                    : new StatChangeData(statEffectProcess.StatProcessName,delta,statEffectProcess.StatModifier.Modifier,
                        CurrentValue));
            }
            else
                _modifierStatEffect.Add(statEffectProcess);

            statEffectProcess.OnDispose += RemoveStatEffect;
            
            return statEffectProcess;
        }
        
        public void ProcessStatModifier(StatModifier statModifier,string statEffectName,PopUpTextConfig textConfig)//may not need to be param
        {
            ProcessStatModifier(statModifier,statEffectName,true,textConfig);
        }
        
        public void ProcessStatModifier(StatModifier statModifier,string statEffectName)//may not need to be param
        {
            ProcessStatModifier(statModifier,statEffectName,false);
        }
        
        private void ProcessStatModifier(StatModifier statModifier,string statEffectName,bool usePopUpText,PopUpTextConfig popUpTextConfig = default)//may not need to be param
        {
            var delta = statModifier.ProcessStatModifier(CurrentValue) - CurrentValue;

            _dynamicValue += delta;
            
            OnValueChanged?.Invoke(usePopUpText
                ? new StatChangeData(statEffectName,delta ,statModifier.Modifier,
                    CurrentValue, popUpTextConfig)
                : new StatChangeData(statEffectName,delta,statModifier.Modifier,
                    CurrentValue));
        }
        
        public void RestStatDynamicValue()
        {
            _dynamicValue = 0;
            OnValueChanged?.Invoke(new StatChangeData($"Reset Stat {Name}",0,CurrentValue));
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

        #endregion

        #region PrivateMethod

        private void RemoveStatEffect(int statProcessInstanceID)
        {
            //TODO: need to find the statEffect and remove it and unsubscribe to the OnDispose event 
            if (FindStatEffectProcess(_processStatEffect,statProcessInstanceID,out var processStatEffect))
            {
                processStatEffect.OnDispose -= RemoveStatEffect;
                _processStatEffect.Remove(processStatEffect);
                _orderProcessStatEffect = _processStatEffect.OrderBy(x => x.StatProcessPriority);//may not be needed
                OnValueChanged?.Invoke(new StatChangeData(processStatEffect.StatProcessName,processStatEffect.StatModifier.Modifier,CurrentValue));
                return;
            }
            if (FindStatEffectProcess(_modifierStatEffect,statProcessInstanceID,out var modifyStatEffect))
            {
                modifyStatEffect.OnDispose -= RemoveStatEffect;
                _modifierStatEffect.Remove(modifyStatEffect);
            }
            //on stat removed
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

        #endregion
    }
    
    public readonly struct StatChangeData
    {
        public readonly string StatEffectName;
        public readonly float Delta;
        public readonly float Modifier;
        public readonly float NewValue;
        public readonly PopUpTextConfig PopUpTextConfig;
        public readonly bool UsePopUpTextConfig;
        public readonly bool HavePopUpTextConfig;
        
        public StatChangeData(string statEffectName,float delta,float modifier, float newValue,PopUpTextConfig popUpTextConfig)
        {
            StatEffectName = statEffectName;
            Modifier = modifier;
            Delta = delta;
            NewValue = newValue;
            UsePopUpTextConfig = true;
            PopUpTextConfig = popUpTextConfig;
            HavePopUpTextConfig = true;
        }
        
        
        public StatChangeData(string statEffectName,float delta, float newValue)
        {
            StatEffectName = statEffectName;
            Delta = delta;
            NewValue = newValue;
            UsePopUpTextConfig = true;
            PopUpTextConfig = default;
            HavePopUpTextConfig = false;
            Modifier = 0;
        }
        
        public StatChangeData(string statEffectName,float delta,float modifier, float newValue)
        {
            StatEffectName = statEffectName;
            Delta = delta;
            Modifier = modifier;
            NewValue = newValue;
            PopUpTextConfig = default;
            HavePopUpTextConfig = false;
            UsePopUpTextConfig = false;
        }
    }
}