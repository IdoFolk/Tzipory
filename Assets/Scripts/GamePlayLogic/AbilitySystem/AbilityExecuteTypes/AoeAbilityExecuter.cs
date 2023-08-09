using System;
using System.Collections.Generic;
using GamePlayLogic.AbilitySystem.AbilityEntity;
using Helpers.Consts;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.SerializeData.AbilitySystemSerializeData;
using Tzipory.SerializeData.StatSystemSerilazeData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.AbilitiesSystem.AbilityExecuteTypes
{
    public class AoeAbilityExecuter :  IAbilityExecutor
    {
        private const string  AoePrefabPath = "Prefabs/Ability/AoeAbilityEntity";
        
        private GameObject _aoePrefab;
        
        public Stat Radius { get; private set; }
        public Stat Duration { get; private set; }
        
        private List<BaseStatusEffect> _statusEffects;
        public AbilityExecuteType AbilityExecuteType => AbilityExecuteType.AOE;
        public IEntityTargetAbleComponent Caster { get; }

        //Changes:
        public List<StatusEffectConfig> OnEnterStatusEffects { get; }
        public List<StatusEffectConfig> OnExitStatusEffects { get; }

        [Obsolete("Use AbilitySerializeData")]
        public AoeAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            OnEnterStatusEffects = new List<StatusEffectConfig>();
            OnExitStatusEffects = new List<StatusEffectConfig>();
            
            OnEnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
            if(abilityConfig.DoExitEffects)
                OnExitStatusEffects.AddRange(abilityConfig.OnExitStatusEffectConfigs);

            Radius = new Stat("AoeRadius", abilityConfig.AoeRadius, int.MaxValue, (int)Constant.Stats.AoeRadius);
            Duration = new Stat("AoeDuration", abilityConfig.AoeDuration, int.MaxValue, (int)Constant.Stats.AoeDuration);

            _aoePrefab = Resources.Load<GameObject>(AoePrefabPath);
        }
        
        public void Init(IEntityTargetAbleComponent target)//temp
        {
            var aoeGameobject = Object.Instantiate(_aoePrefab,target.EntityTransform.position,Quaternion.identity).GetComponent<AoeAbilityEntity>();
            //aoeGameobject.Init(target,Radius.CurrentValue,Duration.CurrentValue,this); //Here the settings need to be changed
            aoeGameobject.Init(target,Radius.CurrentValue,Duration.CurrentValue,this); //Here the settings need to be changed
        }

        public void Execute(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in OnEnterStatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }
        public void ExecuteOnExit(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in OnExitStatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }


    }
}