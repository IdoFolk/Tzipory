using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityMovementComponent : IEntityComponent
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