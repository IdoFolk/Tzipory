using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct AIComponentConfig
    {
        [SerializeField] public AIComponentType AIType;
        [SerializeField] public float DecisionInterval;//temp
        [SerializeField,ShowIf(nameof(AIType),AIComponentType.Enemy)] public float AggroLevel;//temp
        [SerializeField,ShowIf(nameof(AIType),AIComponentType.Enemy)] public float ReturnLevel;//temp
    }

    public enum AIComponentType
    {
        Hero,
        Enemy,
    }
}