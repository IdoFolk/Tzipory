using System;
using SerializeData.StatSerializeData;
using Sirenix.OdinInspector;
using Tzipory.SerializeData.StatSystemSerilazeData;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [Serializable]
    public class Stat
    {
        public event Action<float> OnValueChanged;
        
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private float _currentValue;

        public string Name => _name;
        public float CurrentValue => _currentValue;
        public int Id { get; }
        public float BaseValue { get; }
        public float MaxValue { get; private set; }
        
        [Obsolete("Use StatSerializeData as a parameter")]
        public Stat(StatConfig statConfig)
        {
            _name = statConfig.Name;
            Id = statConfig.Id;
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
            MaxValue = StatLimiters.MaxStatValue; //TBD is this still a thing?
            _currentValue = BaseValue;
        }

        private void ChangeValue(float value)
        {
            _currentValue = value;
            OnValueChanged?.Invoke(_currentValue);
        }

        public void SetValue(float amount) =>
            ChangeValue(amount);

        public void MultiplyValue(float amount)=>
            ChangeValue(_currentValue * amount);
        
        //public void DivideValue(float amount)=> //THIS IS WHY I DON'T LIKE THESE THINGS!
        public void DivideValue(float amount)=>
            ChangeValue(_currentValue / amount);
        
        public void AddToValue(float amount)=>
            ChangeValue(_currentValue + amount);
        
        public void ReduceFromValue(float amount) =>
            ChangeValue(_currentValue - amount);
        
        public void ResetValue() =>
            ChangeValue(BaseValue);
    }
}