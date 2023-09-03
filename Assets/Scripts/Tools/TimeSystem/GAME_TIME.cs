using System;
using UnityEngine;

namespace Tzipory.BaseSystem.TimeSystem
{
    public class GAME_TIME : MonoBehaviour
    {
        public static event Action OnTimeRateChange;
        
        private static float _timeRate = 1f;
        private static float _startGameTime;

        private static float _tempTimeData = 1;
        public static float TimePlayed => Time.realtimeSinceStartup - _startGameTime;
        public static float GetCurrentTimeRate => _timeRate;
        public static float GameDeltaTime => Time.deltaTime * _timeRate;
        public static TimerHandler TimerHandler { get; private set; }

        private void Awake()
        {
            TimerHandler = new TimerHandler();
            _startGameTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            TimerHandler.TickAllTimers();
        }
        
        public static void SetTimeStep(float time)
        {
            if (time < 0)
            {
                Debug.LogError("Can not set timeStep to less or equal to 0");
                return;
            }
            
            _timeRate = time;
            OnTimeRateChange?.Invoke();
        }

        public void ReduseTime()
        {
            SetTimeStep(_timeRate / 2);
        }

        public void AddTime()
        {
            SetTimeStep(_timeRate * 2);
        }

        public static void Play()
        {
            SetTimeStep(_tempTimeData);
            _tempTimeData = 0;
        }
        
        public static void Pause()
        {
            if (_timeRate == 0) return;
            _tempTimeData = _timeRate;
            SetTimeStep(0);
        }
    }
}