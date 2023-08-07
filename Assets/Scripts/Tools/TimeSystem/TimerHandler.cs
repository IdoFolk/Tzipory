using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.BaseSystem.TimeSystem
{
    [Serializable]
    public class TimerHandler
    {
        private Dictionary<string, ITimer> _timersDictionary;
        private List<ITimer> _timersList;

#if UNITY_EDITOR
        [SerializeField, ReadOnly] private List<TimerSerializeData> _timerSerializeDatas;
#endif

        public TimerHandler()
        {
            _timersDictionary = new Dictionary<string, ITimer>();
            _timersList = new List<ITimer>();
#if UNITY_EDITOR
            _timerSerializeDatas = new List<TimerSerializeData>();
#endif
        }

        public ITimer StartNewTimer(float time,Action onComplete,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            Timer timer = new Timer(timerName,time, onComplete);
            
            AddTimer(timer);
            
            return  timer;
        }
        
        public ITimer StartNewTimer<T1>(float time,Action<T1> onComplete,ref T1 parameter ,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1>(timerName,time, onComplete,ref parameter);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2>(float time,Action<T1,T2> onComplete,ref T1 parameter1,ref T2 parameter2,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2>(timerName,time, onComplete,ref parameter1,ref parameter2);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3>(float time,Action<T1,T2,T3> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3,T4>(float time,Action<T1,T2,T3,T4> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3, ref T4 parameter4,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3,T4>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3,ref parameter4);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3,T4,T5>(float time,Action<T1,T2,T3,T4,T5> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3, ref T4 parameter4, ref T5 parameter5,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3,T4,T5>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3,ref parameter4,ref parameter5);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer(float time,string timerName = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer(timerName,time);
            
            AddTimer(timer);
            
            return timer;
        }

        public void StopTimer(ITimer timer)
        {
            if (!string.IsNullOrEmpty(timer.TimerName))
            {
                if (_timersDictionary.TryGetValue(timer.TimerName, out var value))
                {
                    _timersDictionary.Remove(timer.TimerName);
                    return;
                }
            }

            if (_timersList.Contains(timer))
            {
                _timersList.Remove(timer);
                return;
            }

            Debug.LogWarning("can not find a timer");
        }

        private bool ValidateTime(string timerName = null)
        {
            if (!string.IsNullOrEmpty(timerName))
            {
                if (_timersDictionary.ContainsKey(timerName))
                {
                    Debug.LogError("Timer with name " + timerName + " already exists");
                    return false;
                }
            }
            return  true;
        }

        private void AddTimer(ITimer timer)
        {
            if (!string.IsNullOrEmpty(timer.TimerName))
                _timersDictionary.Add(timer.TimerName, timer);
            else
                _timersList.Add(timer);
#if UNITY_EDITOR
            _timerSerializeDatas.Add(new TimerSerializeData(timer));
#endif
            timer.OnTimerComplete += TimeComplete;
        }
        
        public void TickAllTimers()
        {
            foreach (var keyValuePair in _timersDictionary)
                keyValuePair.Value.TickTimer();

            for (int i = 0; i < _timersList.Count; i++)
                _timersList[i].TickTimer();
#if UNITY_EDITOR
            for (int i = 0; i < _timerSerializeDatas.Count; i++)
                _timerSerializeDatas[i].Update();
#endif
        }
        
        private void TimeComplete(ITimer timer)
        {
            
#if UNITY_EDITOR
            for (int i = 0; i < _timerSerializeDatas.Count; i++)
            {
                if (_timerSerializeDatas[i].Timer != timer) continue;
                _timerSerializeDatas.RemoveAt(i);
                break;
            }
#endif
            
            if (!string.IsNullOrEmpty(timer.TimerName))
            {
                if (_timersDictionary.TryGetValue(timer.TimerName, out var value))
                {
                    _timersDictionary.Remove(timer.TimerName);
                    return;
                }

                Debug.LogError($"can not find time by name : {timer.TimerName}");
            }
            
            if (_timersList.Contains(timer))
            {
                _timersList.Remove(timer);
                return;
            }

            Debug.LogError("Could not find time to remove");
        }
    }
}