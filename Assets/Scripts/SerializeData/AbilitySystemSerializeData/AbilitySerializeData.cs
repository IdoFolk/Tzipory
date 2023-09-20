using System.Collections.Generic;
using Tzipory.Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.SerializeData.PlayerData.PartySerializeData.EntitySerializeData;
using Tzipory.SerializeData.StatSystemSerializeData;
using UnityEngine;

namespace Tzipory.SerializeData.AbilitySystemSerializeData
{
    [System.Serializable]
    public class AbilitySerializeData : ISerializeData
    {
        [SerializeField] private int _abilityId;
        [SerializeField] private string _abilityName;
        [SerializeField] private int abilityCastType;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectilePenetration;
        
        [SerializeField] private int abilityExecuteType;
        [SerializeField] private float _aoeRadius;
        [SerializeField] private float _aoeDuration;
        [SerializeField] private float _chainRadius;
        [SerializeField] private float _chainDuration;
        [SerializeField] private float _chainAmount;
        
        [SerializeField] private int targetingPriorityType;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _castTime;
        [SerializeField] private List<StatusEffectSerializeData> _statusEffectConfigs;

        public int AbilityId => _abilityId;

        public string AbilityName => _abilityName;

        public int AbilityCastType => abilityCastType;

        public float ProjectileSpeed => _projectileSpeed;

        public float ProjectilePenetration => _projectilePenetration;

        public int AbilityExecuteType => abilityExecuteType;

        public float AoeRadius => _aoeRadius;

        public float AoeDuration => _aoeDuration;

        public float ChainRadius => _chainRadius;

        public float ChainDuration => _chainDuration;

        public float ChainAmount => _chainAmount;

        public int TargetingPriorityType => targetingPriorityType;

        public float Cooldown => _cooldown;

        public float CastTime => _castTime;

        public List<StatusEffectSerializeData> StatusEffectConfigs => _statusEffectConfigs;

        public int SerializeTypeId => Constant.DataId.ABILITY_DATA_ID;
        

        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}