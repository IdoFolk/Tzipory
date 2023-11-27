using System.Collections.Generic;
using PathCreation;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent.MovementComponents
{
    public class MovementOnPathComponent : IEntityMovementComponent
    {
        private Vector3 _currentPointOnPath;
        
        private PathCreator _pathCreator;
        private PathCreator _finalDestinaion;
        
        private float _privateRabbitIncrement; 
        private float _acceptableDistanceFromPath;
        private float _acceptableDistanceToCompletion;
        
        private float _privateRabbitProgress;
        
        private float _finalLoopSpeed;
        
        public bool CanMove { get; set; }
        public bool IsInitialization { get; private set; }
        public Dictionary<int, Stat> Stats { get; private set; }
        public Stat MovementSpeed => Stats[(int)Constant.StatsId.MovementSpeed];
        
        public bool IsMoving => AgentMoveComponent.IsMoveing;
        public AgentMoveComponent AgentMoveComponent { get; private set; }
        public Vector2 Destination { get; private set; }
        public BaseGameEntity GameEntity { get; private set; }
        
        public void Init(BaseGameEntity parameter1, MovementComponentConfig parameter2,AgentMoveComponent agentMoveComponent)
        {
            Init(parameter1);
            
            AgentMoveComponent = agentMoveComponent;

            Stats = new Dictionary<int, Stat>()
            {
                {
                    (int)Constant.StatsId.MovementSpeed, new Stat(Constant.StatsId.MovementSpeed, parameter2.MoveSpeed)
                },
            };
        }

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }

        public void UpdateComponent()
        {
            if (_pathCreator == null || !CanMove)
                return;

            _currentPointOnPath = _pathCreator.path.GetPointAtDistance(_privateRabbitProgress, EndOfPathInstruction.Stop);

            SetDestination(_currentPointOnPath, MoveType.Guided);

            Vector3 closestPointOnPath = _pathCreator.path.GetClosestPointOnPath(GameEntity.transform.position);

            //if (Vector3.Distance(transform.position, pointOnPath) <= acceptableDistanceFromPath)
            if (Vector3.Distance(GameEntity.transform.position, closestPointOnPath) <= _acceptableDistanceFromPath)
            {
                _privateRabbitProgress += _privateRabbitIncrement;
                if (_privateRabbitProgress > _pathCreator.path.length &&
                    Vector3.Distance(GameEntity.transform.position, _currentPointOnPath) <= _acceptableDistanceToCompletion)
                {
                    _finalDestinaion = LevelManager.CoreTemplete.PatrolPath;
                    CircleFinalDestination();
                }
            }
        }
        
        private void CircleFinalDestination()
        {
            _currentPointOnPath =
                _finalDestinaion.path.GetPointAtDistance(_privateRabbitProgress, EndOfPathInstruction.Loop);
            if (Vector3.Distance(GameEntity.transform.position, _finalDestinaion.path.GetClosestPointOnPath(GameEntity.transform.position)) <=
                _acceptableDistanceToCompletion)
            {
                _privateRabbitProgress += _finalLoopSpeed;
            }

            SetDestination(_currentPointOnPath, MoveType.Free);

            // //TEMP!!!!!!
            // Enemy enemy = GetComponent<Enemy>();
            // enemy.EntityTargetingComponent.SetAttackTarget(LevelManager.CoreTemplete);
            // enemy.IsAttckingCore = true;
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            return new IStatHolder[] { this };
        }

        public void SetDestination(Vector3 destination, MoveType moveType)
        {
            Destination = destination;
            AgentMoveComponent.SetAgentDestination(destination);
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}