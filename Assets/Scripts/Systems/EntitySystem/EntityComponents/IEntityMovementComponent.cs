using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityMovementComponent : IEntityComponent , IInitialization<BaseGameEntity,MovementComponentConfig> , IStatHolder
    {
        public Stat MovementSpeed { get; }

        public void SetDestination(Vector3 destination,MoveType moveType);
    }

    public enum MoveType
    {
        Free,
        Guided
    }
}