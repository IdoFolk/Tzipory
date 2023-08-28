using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.BaseSystem.TimeSystem
{
#if true
    [Serializable]
    public class TimerSerializeData
    {
        [SerializeField, ReadOnly] private string _timerName;
        [SerializeField, ReadOnly] private float _timeRemain;
        [SerializeField, ReadOnly] private string _onCompleted;

        public ITimer Timer { get; }

        public TimerSerializeData(ITimer timer)
        {
            _timerName = timer.TimerName;
            _onCompleted = timer.CompleteMethodName;
            Timer  = timer;
        }

        public void Update() =>
            _timeRemain = Timer.TimeRemaining;
    }
#endif
}