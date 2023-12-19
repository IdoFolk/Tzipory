using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.EntityComponents
{
    public class AgentMoveComponent : MonoBehaviour
    {
        [SerializeField]
        private AgentAuthoring _agent;


        [SerializeField] private float colliderRadius;
        
        private Stat _speedStat;
        
        private Vector3 _lastPosition;

        private Vector2 _destination = Vector2.zero;
        public AgentAuthoring Agent => _agent;
        
        public bool IsMoveing { get; private set; }    
        
        public void Init(Stat newSpeed)
        {
            _speedStat = newSpeed;
            _speedStat.OnValueChanged += AdjustAgentSpeed;
            GAME_TIME.OnTimeRateChange += AdjustAgentTime;

            _agent.enabled = true;
            
            AgentSteering aS = _agent.DefaultSteering;
            aS.Speed = _speedStat.CurrentValue * GAME_TIME.GetCurrentTimeRate;
            _agent.EntitySteering = aS;

            
            _lastPosition = transform.position;
        }
        
        public void SetAgentDestination(Vector3 destination)
        {
            _agent.SetDestination(destination);
            _destination  = destination;
            IsMoveing  = true;
        }

        public void Stop()
        {
            _agent.Stop();
        }

        private void Update()
        {
            if (_destination == Vector2.zero) return;
            
            if (Vector2.Distance(_destination, transform.position) > 0.2f) return;
            
            _destination = Vector2.zero;
            IsMoveing = false;
        }

        private void AdjustAgentSpeed(StatChangeData statChangeData)
        {
            //AgentSteering aS = agent.EntitySteering;
            AgentSteering aS = _agent.DefaultSteering;
            aS.Speed = statChangeData.NewValue * GAME_TIME.GetCurrentTimeRate;
            _agent.EntitySteering = aS;
        }
        
        private void AdjustAgentTime()
        {
            if (_speedStat == null)
                return;
            AgentSteering aS = _agent.DefaultSteering;
            aS.Speed = _speedStat.CurrentValue * GAME_TIME.GetCurrentTimeRate;
            _agent.EntitySteering = aS;
        }
        
        private void OnDisable()
        {
            GAME_TIME.OnTimeRateChange -= AdjustAgentTime;
            if(_speedStat != null)
                _speedStat.OnValueChanged -= AdjustAgentSpeed;
        }

    }
}