using PathCreation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct MovementComponentConfig
    {
        [SerializeField] public MovementComponentType MovementComponentType;
        [SerializeField] public float MoveSpeed;
        [ShowIf(nameof(MovementComponentType), MovementComponentType.GroundMoveOnPath),HideInInspector] public PathCreator PathCreator;
        [SerializeField,ShowIf(nameof(MovementComponentType), MovementComponentType.GroundMoveOnPath)] public float MaxDistanceFromPath;
        [SerializeField,ShowIf(nameof(MovementComponentType), MovementComponentType.GroundMoveOnPath)] public float PrivateRabbitIncrement;
        [SerializeField,ShowIf(nameof(MovementComponentType), MovementComponentType.GroundMoveOnPath)] public float PrivateRabbitDistanceToUpdate;
    }

    public enum MovementComponentType
    {
        GroundMoveOnSelect,
        GroundMoveOnPath,
        Air,
    }
}