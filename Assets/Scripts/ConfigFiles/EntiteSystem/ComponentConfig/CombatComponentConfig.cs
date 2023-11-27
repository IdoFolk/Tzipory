using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct CombatComponentConfig
    {
        [SerializeField] public CombatComponentType CombatComponentType;
        [SerializeField] public float AttackDamage;
        [SerializeField] public float AttackRate;
        [SerializeField,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)] public float AttackRange;
        [SerializeField] public float AttackCooldown;
        [SerializeField] public float CritDamage;
        [SerializeField] public float CritChance;

    }

    public enum CombatComponentType
    {
        Range,
        Melee,
    }
}