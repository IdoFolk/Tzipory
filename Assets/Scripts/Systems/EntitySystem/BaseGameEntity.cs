using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
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

        private List<IEntityComponent> _entityComponent;

        protected bool UpdateComponent;

        private void Awake()
        {
            EntityTimer = new TimerHandler(this);
            EntityTransform = transform;
            EntityInstanceID = InstanceIDGenerator.GetInstanceID();
            _entityComponent = new List<IEntityComponent>();
#if UNITY_EDITOR
            _timerHandler = EntityTimer;
#endif
        }

        private void Update()
        {
            EntityTimer.TickAllTimers();

            if (GAME_TIME.IsTimeStopped)
                return;
            
            foreach (var entityComponent in _entityComponent)
                entityComponent?.UpdateComponent();
        }
        
        public void FocusOnEntity()=>
            GameManager.CameraHandler.SetCameraPosition(transform.position);

        public void AddComponent(IEntityComponent component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            
            _entityComponent.Add(component);
        }

        public void RemoveComponent(IEntityComponent component)
        {
            _entityComponent.Remove(component);//temp need error handle
        }

        public T RequestComponent<T>() where T : class, IEntityComponent
        {
            foreach (var entityComponent in _entityComponent)
            {
                if (entityComponent is T component)
                    return component;
            }

            return null;
        }
    }
}