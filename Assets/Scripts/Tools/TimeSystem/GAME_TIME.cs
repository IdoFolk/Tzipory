using System;
using System.Collections;
using Tzipory.Helpers;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Tools.TimeSystem
{
    public class GAME_TIME : MonoBehaviour
    {
        public const string LOG_GROUP_NAME = "TimeHandler";

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
                Logger.LogError("Can not set timeStep to less or equal to 0");
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
            
            Logger.Log($"Set time to {timeRate}",LOG_GROUP_NAME);
            OnTimeRateChange?.Invoke();
        }

        public static void Play()
        { 
            SetTimeStep(_tempTimeData);
            
            Logger.Log($"<color={ColorLogHelper.GREEN}>PLAY</color>",LOG_GROUP_NAME);
            _tempTimeData = 0;
        }
        
        public static void Pause()
        {
            if (_timeRate == 0) return;
            _tempTimeData = _timeRate;
            
            Logger.Log($"<color={ColorLogHelper.RED}>PLAY</color>",LOG_GROUP_NAME);
            SetTimeStep(0);
        }
    }
}