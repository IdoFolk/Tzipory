using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.Entity
{
    public abstract class BaseGameEntity : MonoBehaviour , IDisposable
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

        protected virtual void Awake()
        {
            EntityTimer = new TimerHandler(this);
            EntityTransform = transform;
            EntityInstanceID = InstanceIDGenerator.GetInstanceID();
            _entityComponent = new List<IEntityComponent>();
#if UNITY_EDITOR
            _timerHandler = EntityTimer;
#endif
        }

        protected virtual void Update()
        {
            EntityTimer.TickAllTimers();
            
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

        public virtual void Dispose()
        {
            foreach (var entityComponent in _entityComponent)
            {
                if (entityComponent is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}