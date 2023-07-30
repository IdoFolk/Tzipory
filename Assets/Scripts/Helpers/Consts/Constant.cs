using SerializeData.StatSerializeData;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.StatusSystem;

namespace Helpers.Consts
{
    public static class Constant
    {
        public static class EffectSequenceIds
        {
            public const int DEATH = 1;
            public const int ATTACK = 2;
            public const int CRIT_ATTACK = 3;
            public const int MOVE = 4;
            public const int GET_HIT = 5;
            public const int GET_CRIT_HIT = 6;
        }
        
        public static class DataId
        {
            public static readonly int ShamanDataID = typeof(ShamanConfig).GetHashCode();
            public static readonly int EnemyDataID = typeof(EnemyConfig).GetHashCode();
            public static readonly int StatDataID = typeof(StatConfig).GetHashCode();
            // public const int ITEM_DATA_ID = typeof(It).GetHashCode();
            public static readonly int AbilityDataID = typeof(AbilityConfig).GetHashCode();
            public static readonly int StatusEffectDataID = typeof(StatusEffectConfig).GetHashCode();
        }


        public static class ShamanId
        {
            public const int  TOOR_ID = 0;
            public const int  NADIA_ID = 1;
            public const int  JAVAN_ID = 2;
            public const int  GUNI_ID = 3;
            public const int  NANA_ID = 4;
        }
        
        public static class EnemyId
        {
            public const int  MUDU_ID = 0;
            public const int  GALLANITE_ID = 1;
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