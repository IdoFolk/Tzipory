using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityMovementComponent : IEntityComponent , IInitialization<BaseGameEntity,MovementComponentConfig,AgentMoveComponent> , IStatHolder , IDisposable
    {
        public Stat MovementSpeed { get; }
        public bool IsMoving { get; }
        public bool CanMove { get; set; }
        public AgentMoveComponent AgentMoveComponent { get; }
        public Vector2 Destination { get; }

        public void SetDestination(Vector3 destination,MoveType moveType);
    }

    public enum MoveType
    {
        Free,
        Guided
    }
}