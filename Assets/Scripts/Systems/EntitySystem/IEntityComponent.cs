using UnityEngine;

namespace Tzipory.Systems.Entity
{
    public interface IEntityComponent
    {
        public int EntityInstanceID { get; }
        public Transform EntityTransform { get; }

        public BaseGameEntity GameEntity { get; }
    }
}