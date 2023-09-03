using System;

namespace Tzipory.EntitySystem.StatusSystem
{
    public class StatModifier
    {
        public StatusModifierType ModifierType { get; }

        private Stat Modifier { get; }

        private float _value;

        private float _previousValue;

        public StatModifier(float modifier,StatusModifierType statusModifierType)
        {
            Modifier = new Stat("Modifier",modifier,int.MaxValue,999);//temp
            ModifierType = statusModifierType;
        }

        public void ProcessStatModifier(Stat stat)
        {
            _previousValue = stat.CurrentValue;
            _value = Modifier.CurrentValue;

            switch (ModifierType)
            {
                case StatusModifierType.Addition:
                    stat.AddToValue(_value);
                    break;
                    //These were missing from the swtich and threw some exceptions that could have cost some time
                    case StatusModifierType.Reduce:
                    stat.ReduceFromValue(_value);
                    break;

                case StatusModifierType.Multiplication:
                    stat.MultiplyValue(_value);
                    break;
                    //These were missing from the swtich and threw some exceptions that could have cost some time
                case StatusModifierType.Divide:
                    stat.DivideValue(_value);
                    break;

                case StatusModifierType.Percentage:
                    //stat.MultiplyValue(_value);//may meed to by change

                    //Alon - temp usage for this: it "Adds" that percentage of itself. It accepts both positive and negative values.
                    //Stat of 10, and a modifier _value of 24 -> will add 24% of the stats currentValue to itself (10 + 24% of 10 -> add [10 * 24/100])
                    //Stat of 10, and a modifier _value of -50 -> will REMOVE 50% of the stats currentValue to itself (10 - 50% of 10 -> subtract [10 * 50/100])
                    //also save previous value so we can UnDo it
                    Modifier.SetValue(stat.CurrentValue);
                    stat.AddToValue(stat.CurrentValue * _value/100f);

                    
                    break;
                case StatusModifierType.Set:
                    stat.SetValue(_value);
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
                    stat.ReduceFromValue(_value);
                    break;
                case StatusModifierType.Multiplication:
                    stat.DivideValue(_value);
                    break;
                case StatusModifierType.Percentage:
                    //stat.DivideValue(_value);//may meed to by change
                    stat.SetValue(Modifier.BaseValue); //returns to original value without any dirty math -> same could be done with StatusModifierType.Set?
                    break;
                case StatusModifierType.Set:
                    //set dos not have a undo
                    stat.SetValue(_previousValue); //returns to previous value

                    break;
                case StatusModifierType.Reduce:
                    //stat.ReduceFromValue(_value); //I flipped those so they would do the opposite operation (not the same)
                    stat.AddToValue(_value);
                    break;
                case StatusModifierType.Divide:
                    //stat.DivideValue(_value);   //I flipped those so they would do the opposite operation (not the same)
                    stat.MultiplyValue(_value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), ModifierType, null);
            }
        }

        public void SetValue(float newValue)
        {
            //_previousValue = _value; //?
            _value = newValue;
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