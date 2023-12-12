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
        
        protected List<IEntityComponent> EntityComponent;

        protected bool UpdateComponent;

        protected virtual void Awake()
        {
            EntityTimer = new TimerHandler(this);
            EntityTransform = transform;
            EntityInstanceID = InstanceIDGenerator.GetInstanceID();
            EntityComponent = new List<IEntityComponent>();
#if UNITY_EDITOR
            _timerHandler = EntityTimer;
#endif
        }

        protected virtual void Update()
        {
            EntityTimer.TickAllTimers();
            
            foreach (var entityComponent in EntityComponent)
                entityComponent?.UpdateComponent();
        }
        
        public void FocusOnEntity()=>
            GameManager.CameraHandler.SetCameraPosition(transform.position);

        public void AddComponent(IEntityComponent component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            
            EntityComponent.Add(component);
        }

        public void RemoveComponent(IEntityComponent component)
        {
            EntityComponent.Remove(component);//temp need error handle
        }

        public T RequestComponent<T>() where T : class, IEntityComponent
        {
            foreach (var entityComponent in EntityComponent)
            {
                if (entityComponent is T component)
                    return component;
            }

            return null;
        }
    }
}