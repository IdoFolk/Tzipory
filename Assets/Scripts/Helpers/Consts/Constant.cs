

using UnityEngine;

namespace Helpers.Consts
{
    public static class Constant
    {
        public static class EffectSequenceIds
        {
            public const int OnDeath = 1;
            public const int OnAttack = 2;
            public const int OnCritAttack = 3;
            public const int OnMove = 4;
            public const int OnGetHit = 5;
            public const int OnGetCritHit = 6;
        }

        public static class VisualConstants
        {
            public static Vector2 DamageRange = new Vector2(10, 150);
            public static Vector2 FontSizeRange = new Vector2(8, 17);
            public static float Crit_FontSizeBonus = 0;

            public static float GetRelativeFontSizeForDamage(float damage) => ( FontSizeRange.y - FontSizeRange.x) * (damage - DamageRange.x)/(DamageRange.y - DamageRange.x) + FontSizeRange.x;
        }

        public enum Stats
        {
            Health,
            AttackDamage,
            AttackRate,
            AttackRange,
            TargetingRange,
            MovementSpeed,
            CritDamage,
            CritChance,
            InvincibleTime,
            AbilityCooldown,
            AbilityCastTime,
            ProjectileSpeed,
            ProjectilePenetration,
            AoeRadius,
            AoeDuration,
            ChainRadius,
            ChainDuration,
            ChainAmount,
        }
    }

    
}