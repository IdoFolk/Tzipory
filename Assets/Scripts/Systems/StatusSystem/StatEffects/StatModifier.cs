using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.StatusSystem;

namespace Tzipory
{
    public readonly struct StatModifier
    {
        public readonly StatModifierType ModifierType;

        public float Modifier { get; }

        public StatModifier(float modifier,StatModifierType statModifierType,bool useDynamicModifier = false)
        {
            ModifierType = statModifierType;
            Modifier = modifier;
        }
        
        public StatModifier(StatModifierConfig statusModifierType,bool useDynamicModifier = false)
        {
            ModifierType = statusModifierType.StatModifierType;
            Modifier = statusModifierType.Modifier;
        }

        public float ProcessStatModifier(float value)
        {
            if (Modifier < 0)
                throw new Exception("StatModifier Modifier is smaller than 0");
            
            switch (ModifierType)
            {
                case StatModifierType.Addition:
                    return value + Modifier;
                case StatModifierType.Reduce:
                    return value - Modifier;
                case StatModifierType.Multiplication:
                    return value * Modifier;
                case StatModifierType.Divide:
                    if (Modifier is 0 or < 0)
                        throw new Exception("StatModifier set to Divide and the value is smaller than 0 or 0");
                    return value / Modifier;
                case StatModifierType.Percentage:
                    float percentage = Modifier / 100;
                    return value * percentage;
                case StatModifierType.SetToZero:
                    return value * 0;//set the value to Zero need to Test
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), ModifierType, null);
            }
        }

        public float Undo(float value)
        {
            switch (ModifierType)
            {
                case StatModifierType.Addition:
                    return value - Modifier;
                case StatModifierType.Multiplication:
                    return value / Modifier;
                case StatModifierType.Percentage:
                    return value / Modifier;
                case StatModifierType.SetToZero:
                    return value - Modifier;
                case StatModifierType.Reduce:
                    return value + Modifier;
                case StatModifierType.Divide:
                    return value * Modifier;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType),ModifierType,null);
            }
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            throw new NotImplementedException();
        }
    }
        
    public enum StatModifierType
    {
        SetToZero,
        Addition,
        Reduce,
        Multiplication,
        Divide,
        Percentage
    }
}