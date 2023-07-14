using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [System.Serializable]
    public class Stat
    {
        //TEMP!
        public event Action<float> OnValueChanged;
        
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private int _id;
        [SerializeField] private float _baseValue;
        [SerializeField,ReadOnly] float _currentValue;

        public string Name
        {
            get => _name;
#if UNITY_EDITOR
            set => _name = value;
#endif
        }
        public int Id
        {
            get => _id;
#if UNITY_EDITOR
            set => _id = value;
#endif
        }
        public float BaseValue => _baseValue;
        public float CurrentValue => _currentValue;
        
        public float MaxValue { get; private set; }

        
        public Stat(string name, float baseValue,float maxValue,int id)
        {
            _name = name;
            _id = id;  
            _baseValue = baseValue;
            MaxValue = StatLimiters.MaxStatValue; //TBD is this still a thing?
            _currentValue = _baseValue;
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
            ChangeValue(_baseValue);
    }
}