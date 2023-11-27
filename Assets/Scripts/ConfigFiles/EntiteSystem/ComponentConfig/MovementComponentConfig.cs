using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct MovementComponentConfig
    {
        [SerializeField] public MovementComponentType MovementComponentType;
        [SerializeField] public float MoveSpeed;
    }

    public enum MovementComponentType
    {
        Ground,
        Air,
    }
}