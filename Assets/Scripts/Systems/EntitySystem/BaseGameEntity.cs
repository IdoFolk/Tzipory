using Sirenix.OdinInspector;
using Tzipory.Tools;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.Entity
{
    public abstract class BaseGameEntity : MonoBehaviour
    {
        public const string ENTITY_LOG_GROUP = "Entity";
        
#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("Timers")] private TimerHandler _timerHandler; 
#endif
        
        public int EntityInstanceID { get; private set; }
        public Transform EntityTransform { get; private set; }
        public TimerHandler EntityTimer { get; private set; }
        public BaseGameEntity GameEntity => this;

        protected virtual void Awake()
        {
            EntityTimer = new TimerHandler();
            EntityTransform = transform;
            EntityInstanceID = InstanceIDGenerator.GetInstanceID();
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