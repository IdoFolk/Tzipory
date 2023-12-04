using System;
using System.Collections;
using Tzipory.Helpers;
using UnityEngine;

namespace Tzipory.Tools
{
    public class LerpValuesHandler : MonoSingleton<LerpValuesHandler>
    {
        private static AnimationCurve _defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private LerpValueConfig _defaultConfig;

        public void SetValueByType<T>(LerpValueConfig config, T type, float currentValue, float newValue, Action<T, float> setter)
            where T : Enum
        {
            StartCoroutine(LerpBetweenValues<T>(config, type, currentValue, newValue, setter));
        }

        public void SetValueByType<T>(T type, float currentValue, float newValue, Action<T, float> setter) where T : Enum
        {
            StartCoroutine(LerpBetweenValues<T>(_defaultConfig, type, currentValue, newValue, setter));
        }

        private IEnumerator LerpBetweenValues<T>(LerpValueConfig config, T type, float currentValue, float newValue,
            Action<T, float> setter) where T : Enum
        {
            float transitionTimeCount = 0;
            var animationCurve = config.TransitionCurve ?? _defaultCurve;
            while (transitionTimeCount < config.TransitionTime)
            {
                transitionTimeCount += Time.deltaTime;

                float evaluateValue = animationCurve.Evaluate(transitionTimeCount / config.TransitionTime);

                float value = Mathf.Lerp(currentValue, newValue, evaluateValue);
                setter.Invoke(type, value);

                yield return null;
            }
        }
    }

    [Serializable]
    public class LerpValueConfig
    {
        public AnimationCurve TransitionCurve;
        public float TransitionTime;
    }
}