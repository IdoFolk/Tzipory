using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory;
using Tzipory.Helpers.Consts;
using UnityEngine;

[Serializable]
public class SimpleStat
{
    public event Action<float> OnValueChanged;

    public Constant.StatsId Name { get; }
    public int Id { get; }
    public float BaseValue { get; private set; }
    public float CurrentValue { get; private set; }
    
#if UNITY_EDITOR //only for debug in the editor
    [SerializeField,ReadOnly] private string _name => Name.ToString();
    [SerializeField,ReadOnly] private float _currentValue => CurrentValue;
#endif

    public SimpleStat(StatConfig statConfig)
    {
        Name = statConfig.Name;
        BaseValue = statConfig.Value;
        Id = (int)Name;

        CurrentValue = BaseValue;
    }

    public void ChangeValue(float modifier, StatModifierType modifierType)
    {
        bool valueChanged = true;
        float oldCurrentValue = CurrentValue;
        switch (modifierType)
        {
            case StatModifierType.SetToZero:
                CurrentValue = 0;
                break;
            case StatModifierType.Addition:
                CurrentValue += modifier;
                break;
            case StatModifierType.Reduce:
                CurrentValue -= modifier;
                break;
            case StatModifierType.Multiplication:
                CurrentValue *= modifier;
                break;
            case StatModifierType.Divide:
                if (CurrentValue is 0 or < 0)
                    throw new Exception("StatModifier set to Divide and the value is smaller than 0 or 0");
                CurrentValue /= modifier;
                break;
            case StatModifierType.Percentage:
                float percentage = modifier / 100;
                CurrentValue *= percentage;
                break;
            default:
                valueChanged = false;
                break;
        }

        float delta = CurrentValue - oldCurrentValue;
       if (valueChanged) OnValueChanged?.Invoke(delta);
    }
}

[Serializable]
public struct StatConfig
{
    public Constant.StatsId Name;
    public float Value { get; private set; }
}
