using System;
using System.Collections.Generic;
using Tzipory.Tools;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    public abstract class EffectTransitionLerp<T> where T: Enum
    {
        [SerializeField] private LerpValueConfig LerpValueConfig;
        [SerializeField] private List<EffectTransitionLerpValue<T>> EffectValues;

        public virtual void StartTransitionEffect()
        {
            foreach (var effectValue in EffectValues)
            {
                LerpValuesHandler.Instance.SetValueByType(LerpValueConfig,effectValue.ValueType,effectValue.DefaultValue,effectValue.SlowMotionValue,SetValue);
            }
        }
        public virtual void EndTransitionEffect()
        {
            foreach (var effectValue in EffectValues)
            {
                LerpValuesHandler.Instance.SetValueByType(LerpValueConfig,effectValue.ValueType,effectValue.SlowMotionValue,effectValue.DefaultValue,SetValue);
            }
        }

        protected abstract void SetValue(T type,float value);
    }
    
    [Serializable]
    public struct EffectTransitionLerpValue<T> where T : Enum
    {
        public T ValueType;
        public float DefaultValue;
        public float SlowMotionValue;
    }
}