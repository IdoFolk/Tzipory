using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.MovementSystem.HerosMovementSystem;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent.MovementComponents
{
    public class OnSelectMoveComponent : IEntityMovementComponent
    {
        private ClickHelper _clickHelper;
        
        public bool IsInitialization { get; private set; }
        public Dictionary<int, Stat> Stats { get; private set; }
        public Stat MovementSpeed => Stats[(int)Constant.StatsId.MovementSpeed];
        public AgentMoveComponent AgentMoveComponent { get; private set; }
        public Vector2 Destination { get; private set; }
        public bool IsMoving => AgentMoveComponent.IsMoveing;
        public bool CanMove { get; set; }
        public BaseGameEntity GameEntity { get; private set; }
        
        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }
        
        public void Init(BaseGameEntity parameter1, MovementComponentConfig config,AgentMoveComponent agentMoveComponent)
        {
            Init(parameter1);
            
            Stats = new Dictionary<int, Stat>()
            {
                {
                    (int)Constant.StatsId.MovementSpeed, new Stat(Constant.StatsId.MovementSpeed, config.MoveSpeed)
                },
            };
            
            AgentMoveComponent = agentMoveComponent;
            
            _clickHelper = GameEntity.GetComponentInChildren<ClickHelper>();//Temp
            
            _clickHelper.OnClick += SelectHero;
            
            IsInitialization = true;
        }

        private void SelectHero()
        {
            UnitEntity unitEntity = (UnitEntity)GameEntity;
            
            TempHeroMovementManager.Instance.SelectTarget(AgentMoveComponent,unitEntity.EntityVisualComponent.SpriteRenderer.sprite, unitEntity.EntityTargetingComponent.TargetingRange.CurrentValue);
        }

        public void UpdateComponent()
        {
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            throw new System.NotImplementedException();
        }

        public void SetDestination(Vector3 destination, MoveType moveType)
        {
            Destination = destination;
            AgentMoveComponent.SetAgentDestination(destination);
        }

        public void Dispose()
        {
            _clickHelper.OnClick -= SelectHero;
        }
    }
}