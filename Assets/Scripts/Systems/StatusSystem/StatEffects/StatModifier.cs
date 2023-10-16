using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.Systems.StatusSystem
{
    public readonly struct StatModifier
    {
        public readonly StatusModifierType ModifierType;

        public float Modifier { get; }

        public StatModifier(float modifier,StatusModifierType statusModifierType,bool useDynamicModifier = false)
        {
            ModifierType = statusModifierType;
            Modifier = modifier;
        }
        
        public StatModifier(StatModifierConfig statusModifierType,bool useDynamicModifier = false)
        {
            ModifierType = statusModifierType.StatusModifierType;
            Modifier = statusModifierType.Modifier;
        }

        public float ProcessStatModifier(float value)
        {
            if (Modifier < 0)
                throw new Exception("StatModifier Modifier is smaller than 0");
            
            switch (ModifierType)
            {
                case StatusModifierType.Addition:
                    return value + Modifier;
                case StatusModifierType.Reduce:
                    return value - Modifier;
                case StatusModifierType.Multiplication:
                    return value * Modifier;
                case StatusModifierType.Divide:
                    if (Modifier is 0 or < 0)
                        throw new Exception("StatModifier set to Divide and the value is smaller than 0 or 0");
                    return value / Modifier;
                case StatusModifierType.Percentage:
                    float percentage = Modifier / 100;
                    return value * percentage;
                case StatusModifierType.SetToZero:
                    return value * 0;//set the value to Zero need to Test
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), ModifierType, null);
            }
        }

        public float Undo(float value)
        {
            switch (ModifierType)
            {
                case StatusModifierType.Addition:
                    return value - Modifier;
                case StatusModifierType.Multiplication:
                    return value / Modifier;
                case StatusModifierType.Percentage:
                    return value / Modifier;
                case StatusModifierType.SetToZero:
                    return value - Modifier;
                case StatusModifierType.Reduce:
                    return value + Modifier;
                case StatusModifierType.Divide:
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
        
    public enum StatusModifierType
    {
        SetToZero,
        Addition,
        Reduce,
        Multiplication,
        Divide,
        Percentage
    }
}