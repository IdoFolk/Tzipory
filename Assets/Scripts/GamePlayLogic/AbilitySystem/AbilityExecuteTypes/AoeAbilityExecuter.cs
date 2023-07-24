using System.Collections.Generic;
using Helpers.Consts;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

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
        public List<StatusEffectConfig> StatusEffects { get; }


        public AoeAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            StatusEffects = new List<StatusEffectConfig>();
            
            StatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
            
            Radius = new Stat("AoeRadius", abilityConfig.AoeRadius, int.MaxValue, (int)Constant.Stats.AoeRadius);
            Duration = new Stat("AoeDuration", abilityConfig.AoeDuration, int.MaxValue, (int)Constant.Stats.AoeDuration);

            _aoePrefab = Resources.Load<GameObject>(AoePrefabPath);
        }

        public void Init(IEntityTargetAbleComponent target)//temp
        {
            var aoeGameobject = Object.Instantiate(_aoePrefab,target.EntityTransform.position,Quaternion.identity).GetComponent<AoeAbilityEntity>();
            aoeGameobject.Init(target,Radius.CurrentValue,Duration.CurrentValue,this);
        }

        public void Execute(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in StatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }

    }
}