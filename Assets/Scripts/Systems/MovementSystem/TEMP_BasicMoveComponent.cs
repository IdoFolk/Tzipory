using System;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.EntityComponents
{
    public class TEMP_BasicMoveComponent : MonoBehaviour, IEntityMovementComponent
    {
        [SerializeField]
        private AgentAuthoring agent;
        
        private Stat _speedStat;

        private float _speed;
        
        private Vector3 _lastPosition;

        private Vector2 _destination = Vector2.zero;

        private Action oncomplet;

        //Set/init by Unit

        //public float AdjustedSpeed => _speedStat.CurrentValue * GAME_TIME.TimeRate;
        //public float AdjustedSpeed => _speedStat.CurrentValue * GAME_TIME;
        public void Init(Stat newSpeed)
        {
            _speedStat = newSpeed;
            _speedStat.OnValueChanged += AdjustAgentSpeed;
            
            AgentSteering aS = agent.DefaultSteering;
            aS.Speed = _speedStat.CurrentValue * GAME_TIME.GetCurrentTimeRate;
            agent.EntitySteering = aS;
            
            _lastPosition = transform.position;
        }

        public Stat MovementSpeed => _speedStat;

        public int EntityInstanceID => throw new NotImplementedException();

        public Transform EntityTransform => agent.transform;

        public BaseGameEntity GameEntity => throw new NotImplementedException();

        public bool IsMoveing { get; private set; }    

        public void SetDestination(Vector3 destination, MoveType moveType,Action oncomplete = null)
        {
            agent.SetDestination(destination);
            _destination  = destination;
            IsMoveing  = true;

            this.oncomplet = oncomplete;
        }

        public void Stop()
        {
            agent.Stop();
        }

        private void Update()
        {
            if (_destination == Vector2.zero) return;
            
            if (Vector2.Distance(_destination, transform.position) > 0.2f) return;

            if (!IsMoveing) return;
            
            oncomplet?.Invoke();
            _destination = Vector2.zero;
            IsMoveing = false;
        }

        private void FixedUpdate()
        { 
            if (GAME_TIME.GameDeltaTime == 0)
                _speed = 0;
            else
                _speed = Vector3.Distance(transform.position, _lastPosition) / Time.fixedDeltaTime;
            
            _lastPosition = transform.position;
        }

        void AdjustAgentSpeed(StatChangeData statChangeData) //subs to OnTimeRateChange
        {
            //AgentSteering aS = agent.EntitySteering;
            AgentSteering aS = agent.DefaultSteering;
            aS.Speed = statChangeData.NewValue * GAME_TIME.GetCurrentTimeRate;
            agent.EntitySteering = aS;
        }
        
        void AdjustAgentTime() //subs to OnTimeRateChange
        {
            if (_speedStat == null)
                return;
            AgentSteering aS = agent.DefaultSteering;
            aS.Speed = _speedStat.CurrentValue * GAME_TIME.GetCurrentTimeRate;
            agent.EntitySteering = aS;
        }

        private void OnEnable()
        {
            GAME_TIME.OnTimeRateChange += AdjustAgentTime; //?
        }
        private void OnDisable()
        {
            GAME_TIME.OnTimeRateChange -= AdjustAgentTime;
            if(_speedStat != null)
                _speedStat.OnValueChanged -= AdjustAgentSpeed;
        }

    }
}