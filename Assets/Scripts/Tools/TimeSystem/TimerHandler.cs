using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Tzipory.BaseSystem.TimeSystem
{
    [Serializable]
    public class TimerHandler
    {
        private List<ITimer> _timersList;

#if UNITY_EDITOR
        [SerializeField, ReadOnly] private List<TimerSerializeData> _timerSerializeDatas;
#endif

        public TimerHandler()
        {
            _timersList = new List<ITimer>();
#if UNITY_EDITOR
            _timerSerializeDatas = new List<TimerSerializeData>();
#endif
        }

        public ITimer StartNewTimer(float time,string timerName,Action onComplete = null)
        {
            if (!ValidateTime(timerName))
                return null;
            
            Timer timer = new Timer(timerName,time, onComplete);
            
            AddTimer(timer);
            
            return  timer;
        }
        
        public ITimer StartNewTimer<T1>(float time,string timerName,Action<T1> onComplete,ref T1 parameter)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1>(timerName,time, onComplete,ref parameter);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2>(float time,string timerName,Action<T1,T2> onComplete,ref T1 parameter1,ref T2 parameter2)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2>(timerName,time, onComplete,ref parameter1,ref parameter2);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3>(float time,string timerName,Action<T1,T2,T3> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3,T4>(float time,string timerName,Action<T1,T2,T3,T4> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3, ref T4 parameter4)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3,T4>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3,ref parameter4);
            
            AddTimer(timer);
            
            return timer;
        }
        
        public ITimer StartNewTimer<T1,T2,T3,T4,T5>(float time,string timerName, System.Action<T1,T2,T3,T4,T5> onComplete,ref T1 parameter1,ref T2 parameter2, ref T3 parameter3, ref T4 parameter4, ref T5 parameter5)
        {
            if (!ValidateTime(timerName))
                return null;
            
            var timer = new Timer<T1,T2,T3,T4,T5>(timerName,time, onComplete,ref parameter1,ref parameter2,ref parameter3,ref parameter4,ref parameter5);
            
            AddTimer(timer);
            
            return timer;
        }

        private bool ValidateTime(string timerName = null)
        {
            //not in use for now
            return true;
        }

        private void AddTimer(ITimer timer)
        {
            _timersList.Add(timer);
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Timer Handler:</color><color={ColorLogHelper.GREEN}> Start</color> timer {timer.TimerName} at time {timer.TimeRemaining}");
            _timerSerializeDatas.Add(new TimerSerializeData(timer));
#endif
            timer.OnTimerComplete += TimeComplete;
        }
        
        public void TickAllTimers()
        {
            for (int i = 0; i < _timersList.Count; i++)
                _timersList[i].TickTimer();
#if UNITY_EDITOR
            for (int i = 0; i < _timerSerializeDatas.Count; i++)
                _timerSerializeDatas[i].Update();
#endif
        }
        
        private void TimeComplete(ITimer timer,bool isStopped)
        {

#if UNITY_EDITOR
            if (isStopped)
                Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Timer Handler:</color><color={ColorLogHelper.RED}>Stop</color> timer {timer.TimerName} at time reminding: {timer.TimeRemaining}");
            else
                Debug.Log($"<color={ColorLogHelper.TIMER_HANDLER_COLOR}>Timer Handler:</color><color={ColorLogHelper.RED}>Complete</color> timer {timer.TimerName} at time reminding: {timer.TimeRemaining}");
#endif
            
#if UNITY_EDITOR
            for (int i = 0; i < _timerSerializeDatas.Count; i++)
            {
                if (_timerSerializeDatas[i].Timer != timer) continue;
                _timerSerializeDatas.RemoveAt(i);
                break;
            }
#endif
            if (_timersList.Contains(timer))
            {
                _timersList.Remove(timer);
                return;
            }

            Debug.LogError("Could not find time to remove");
        }
    }
}