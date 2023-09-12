using System;
using System.Collections;
using Helpers;
using UnityEngine;

namespace Tzipory.BaseSystem.TimeSystem
{
    public class GAME_TIME : MonoBehaviour
    {
        public static event Action OnTimeRateChange;
        
        private static float _timeRate = 1f;
        private static float _startGameTime;
        
        private static AnimationCurve _defaultCurve = AnimationCurve.Linear(0,0,1,1);

        private static float _tempTimeData = 1;
        public static float TimePlayed => Time.realtimeSinceStartup - _startGameTime;
        public static float GetCurrentTimeRate => _timeRate;
        public static float GameDeltaTime => Time.deltaTime * _timeRate;
        public static TimerHandler TimerHandler { get; private set; }

        private static MonoBehaviour _monoBehaviour;

        private static Coroutine _fadeCoroutine;

        private void Awake()
        {
            _monoBehaviour = this;
            TimerHandler = new TimerHandler();
            _startGameTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            TimerHandler.TickAllTimers();
        }
        
        public static void SetTimeStep(float time,float transitionTime = 1 ,AnimationCurve curve = null)
        {
            if (time < 0)
            {
                Debug.LogError("Can not set timeStep to less or equal to 0");
                return;
            }

            if (_fadeCoroutine != null)
            {
                _monoBehaviour.StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }

            if (curve == null)
                SetTime(time);
            else
                _fadeCoroutine = _monoBehaviour.StartCoroutine(FadeTime(time, transitionTime, curve));
        }
        
        private static IEnumerator FadeTime(float time,float transitionTime = 1 ,AnimationCurve curve = null)
        {
            float currentTimeRate = GetCurrentTimeRate;
            float transitionTimeCount = 0;
            var animationCurve = curve ?? _defaultCurve;

            while (transitionTimeCount < transitionTime)
            {
                transitionTimeCount += Time.deltaTime;

                float evaluateValue = animationCurve.Evaluate(transitionTimeCount / transitionTime);

                _timeRate = Mathf.Lerp(currentTimeRate, time, evaluateValue);
                
                yield return null;
            }
            
            SetTime(time);
        }

        private static void SetTime(float timeRate)
        {
            _timeRate = timeRate;
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Time Handler:</color> set time to {timeRate}");
#endif
            OnTimeRateChange?.Invoke();
        }

        public static void Play()
        { 
            SetTimeStep(_tempTimeData);
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Time Handler:</color> <color={ColorLogHelper.GREEN}>PLAY</color>");
#endif
            _tempTimeData = 0;
        }
        
        public static void Pause()
        {
            if (_timeRate == 0) return;
            _tempTimeData = _timeRate;
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Time Handler:</color> <color={ColorLogHelper.RED}>PLAY</color>");
#endif
            SetTimeStep(0);
        }
    }
}