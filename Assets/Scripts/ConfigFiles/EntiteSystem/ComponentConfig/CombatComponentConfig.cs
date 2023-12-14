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
        [SerializeField] public float AttackCooldown;
        [SerializeField] public float CritDamage;
        [SerializeField] public float CritChance;
        [SerializeField,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)] public float AttackRange;
        [Header("Projectile config"),ShowIf(nameof(CombatComponentType), CombatComponentType.Range)]
        [Space,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)]
        [SerializeField,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)] public float ProjectileTimeToDie;
        [SerializeField,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)] public float ProjectileSpeed;
        [SerializeField,ShowIf(nameof(CombatComponentType), CombatComponentType.Range)] public Temp_Projectile ProjectilePrefab;

    }

    public enum CombatComponentType
    {
        Range,
        Melee,
    }
}