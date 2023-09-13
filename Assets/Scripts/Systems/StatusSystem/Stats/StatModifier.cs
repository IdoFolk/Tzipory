using System;
using UnityEngine;

namespace Tzipory.ConfigFiles.VisualSystemConfig
{
    public readonly struct StatModifier
    {
        public readonly StatusModifierType ModifierType;
        
        public readonly float Value;
        
        public StatModifier(float modifier,StatusModifierType statusModifierType)
        {
            Value = modifier;
            ModifierType = statusModifierType;
        }

        public void ProcessStatModifier(Stat stat)
        {
            switch (ModifierType)
            {
                case StatusModifierType.Addition:
                    stat.AddToValue(Value);
                    break;
                    case StatusModifierType.Reduce:
                    stat.ReduceFromValue(Value);
                    break;
                case StatusModifierType.Multiplication:
                    stat.MultiplyValue(Value);
                    break;
                case StatusModifierType.Divide:
                    stat.DivideValue(Value);
                    break;
                case StatusModifierType.Percentage:
                    stat.AddToValue(stat.CurrentValue * Value / 100f);
                    break;
                case StatusModifierType.Set:
                    stat.SetValue(Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), ModifierType, null);
            }
        }

        public void Undo(Stat stat)
        {
            switch (ModifierType)
            {
                case StatusModifierType.Addition:
                    stat.ReduceFromValue(Value);
                    break;
                case StatusModifierType.Multiplication:
                    stat.DivideValue(Value);
                    break;
                case StatusModifierType.Percentage:
                    stat.SetValue(Value);
                    break;
                case StatusModifierType.Set:
                    stat.ResetSetValue();
                    break;
                case StatusModifierType.Reduce:
                    stat.AddToValue(Value);
                    break;
                case StatusModifierType.Divide:
                    stat.MultiplyValue(Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), ModifierType, null);
            }
        }
    }
        
    public enum StatusModifierType
    {
        Set,
        Addition,
        Reduce,
        Multiplication,
        Divide,
        Percentage
    }
}