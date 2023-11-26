using System;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityMovementComponent : IEntityComponent
    {
        public Stat MovementSpeed { get; }

        public void SetDestination(Vector3 destination,MoveType moveType,Action<Vector3> onComplete);
    }

    public enum MoveType
    {
        Free,
        Guided
    }
}