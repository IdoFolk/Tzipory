using Sirenix.OdinInspector;
using Tzipory.BaseSystem.TimeSystem;
using UnityEngine;

namespace Tzipory.EntitySystem
{
    public abstract class BaseGameEntity : MonoBehaviour , IEntityComponent
    {
#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("Timers")] private TimerHandler _timerHandler; 
#endif
        
        public int EntityInstanceID { get; private set; }
        public Transform EntityTransform { get; private set; }
        public TimerHandler EntityTimer { get; private set; }

        public Ticker Ticker { get; } //need to implement
        public BaseGameEntity GameEntity => this;

        protected virtual void Awake()
        {
            EntityTimer = new TimerHandler();
            EntityTransform = transform;
            EntityInstanceID = EntityIDGenerator.GetInstanceID();
#if UNITY_EDITOR
            _timerHandler = EntityTimer;
#endif
        }

        protected virtual void Update()
        {
            EntityTimer.TickAllTimers();
        }
    }
    
}