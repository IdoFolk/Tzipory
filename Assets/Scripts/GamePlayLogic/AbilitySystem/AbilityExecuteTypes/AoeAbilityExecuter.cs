using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.AbilitySystem.AbilityEntity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class AoeAbilityExecuter :  IAbilityExecutor , IStatHolder
    {
        private const string  AoePrefabPath = "Prefabs/Ability/AoeAbilityEntity";
        
        private GameObject _aoePrefab;


        private Stat Radius
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AoeRadius, out var aoeRadius))
                    return aoeRadius;

                throw new Exception($"aoeRadius not found in entity {Caster.GameEntity.name}");
            }
        }

        private Stat Duration
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AoeDuration, out var aoeDuration))
                    return aoeDuration;

                throw new Exception($"aoeDuration not found in entity {Caster.GameEntity.name}");
            }
        }

        private List<BaseModifyStatEffect> _statusEffects;
        public AbilityExecuteType AbilityExecuteType => AbilityExecuteType.AOE;
        public IEntityTargetAbleComponent Caster { get; }

        //Changes:
        public List<StatEffectConfig> OnEnterStatusEffects { get; }
        public List<StatEffectConfig> OnExitStatusEffects { get; }

        [Obsolete("Use AbilitySerializeData")]
        public AoeAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Stats = new Dictionary<int, Stat>();

            Caster = caster;
            OnEnterStatusEffects = new List<StatEffectConfig>();
            OnExitStatusEffects = new List<StatEffectConfig>();
            
            OnEnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
            if(abilityConfig.DoExitEffects)
                OnExitStatusEffects.AddRange(abilityConfig.OnExitStatusEffectConfigs);
            
            Stats.Add((int)Constant.StatsId.AoeRadius,new Stat("AoeRadius", abilityConfig.AoeRadius, int.MaxValue, (int)Constant.StatsId.AoeRadius));
            Stats.Add((int)Constant.StatsId.AoeDuration, new Stat("AoeDuration", abilityConfig.AoeDuration, int.MaxValue, (int)Constant.StatsId.AoeDuration));

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
            if (target.EntityType == Caster.EntityType)
                return;

            foreach (var statusEffect in OnEnterStatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }
        public void ExecuteOnExit(IEntityTargetAbleComponent target)
        {
            if (target.EntityType == Caster.EntityType)
                return;

            foreach (var statusEffect in OnExitStatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }


        public Dictionary<int, Stat> Stats { get; }
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
    }
}