using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.Tools.Sound;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{

    public class WindEffectHandler : MonoBehaviour
    {
        private static AnimationCurve _defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private ParticleSystem _curlyWindParticles;
        [SerializeField] private ParticleSystem _basicWindParticles;
        [SerializeField] private ParticleAnimationValue[] _particleAnimationValues;

        private void OnValidate()
        {
            _curlyWindParticles ??= GetComponent<ParticleSystem>();
            _basicWindParticles ??= GetComponent<ParticleSystem>();
        }

        public void SetSlowMotionParticleAnimation(float transitionTime = 1, AnimationCurve curve = null)
        {
            foreach (var particleAnimation in _particleAnimationValues)
            {
                StartCoroutine(FadeParticleAnimation(particleAnimation.Type, particleAnimation.DefaultValue,
                    particleAnimation.SlowMotionValue, transitionTime, curve));
            }
        }
        public void EndSlowMotionParticleAnimation(float transitionTime = 1, AnimationCurve curve = null)
        {
            foreach (var particleAnimation in _particleAnimationValues)
            {
                StartCoroutine(FadeParticleAnimation(particleAnimation.Type, particleAnimation.SlowMotionValue,
                    particleAnimation.DefaultValue, transitionTime, curve));
            }
        }
        private IEnumerator FadeParticleAnimation(WindParticleAnimationType type, float oldValue, float newValue, float transitionTime,
            AnimationCurve curve)
        {
            float transitionTimeCount = 0;
            var animationCurve = curve ?? _defaultCurve;

            while (transitionTimeCount < transitionTime)
            {
                transitionTimeCount += Time.deltaTime;

                float evaluateValue = animationCurve.Evaluate(transitionTimeCount / transitionTime);

                float value = Mathf.Lerp(oldValue, newValue, evaluateValue);
                SetWindSpeedValue(type, value);

                yield return null;
            }

            SetWindSpeedValue(type, newValue);
        }
        private void SetWindSpeedValue(WindParticleAnimationType type, float value)
        {
            switch (type)
            {
                case WindParticleAnimationType.Curly:
                    //_curlyWindParticles.main.simulationSpeed = value;
                    break;
                case WindParticleAnimationType.Basic:
                    //_basicWindParticles.main.simulationSpeed = value;
                    break;
            }
        }
    }

    [Serializable]
    public struct ParticleAnimationValue
    {
        public WindParticleAnimationType Type;
        public float DefaultValue;
        public float SlowMotionValue;
    }

    public enum WindParticleAnimationType
    {
        Curly,
        Basic
    }
}