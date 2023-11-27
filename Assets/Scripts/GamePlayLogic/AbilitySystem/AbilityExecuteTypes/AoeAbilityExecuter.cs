using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.ConfigFiles.Visual;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.AbilitySystem.AbilityEntity;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class AoeAbilityExecuter :  IAbilityExecutor , IStatHolder
    {
        private const string  AOE_PREFAB_PATH = "Prefabs/Ability/AoeAbilityEntity";
        
        private readonly GameObject _aoePrefab;

        public readonly AnimationConfig AnimationConfig;

        public readonly PlayableAsset Visual;
        
        private List<BaseModifyStatEffect> _statusEffects;
        public AbilityExecuteType AbilityExecuteType => AbilityExecuteType.AOE;
        public ITargetAbleEntity Caster { get; }

        //Changes:
        public List<StatEffectConfig> EnterStatusEffects { get; }
        public List<StatEffectConfig> ExitStatusEffects { get; }
        
        public Dictionary<int, Stat> Stats { get; }
        
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

        [Obsolete("Use AbilitySerializeData")]
        public AoeAbilityExecuter(ITargetAbleEntity caster,AbilityConfig abilityConfig)
        {
            Stats = new Dictionary<int, Stat>();

            Caster = caster;
           
            EnterStatusEffects = new List<StatEffectConfig>();
            ExitStatusEffects = new List<StatEffectConfig>();

            AnimationConfig = abilityConfig.AnimationConfig;
            
            EnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
            if(abilityConfig.DoExitEffects)
                ExitStatusEffects.AddRange(abilityConfig.OnExitStatusEffectConfigs);
            
            Stats.Add((int)Constant.StatsId.AoeRadius,new Stat("AoeRadius", abilityConfig.AoeRadius, int.MaxValue, (int)Constant.StatsId.AoeRadius));
            Stats.Add((int)Constant.StatsId.AoeDuration, new Stat("AoeDuration", abilityConfig.AoeDuration, int.MaxValue, (int)Constant.StatsId.AoeDuration));
            
            _aoePrefab = Resources.Load<GameObject>(AOE_PREFAB_PATH);
        }
        
        public void Init(ITargetAbleEntity target)//temp
        {
            var aoeGameObject = Object.Instantiate(_aoePrefab).GetComponent<AoeAbilityEntity>();
            aoeGameObject.gameObject.SetActive(false);
            aoeGameObject.Init(Radius.CurrentValue,Duration.CurrentValue,this); //Here the settings need to be changed
        }

        public void Execute(ITargetAbleEntity target)
        {
            if (target.EntityType == Caster.EntityType)
                return;

            foreach (var statusEffect in EnterStatusEffects)
                target.EntityStatComponent.AddStatEffect(statusEffect);
            
            target.EntityVisualComponent.StartAnimationEffect(AnimationConfig);
        }
        
        public void ExecuteOnExit(ITargetAbleEntity target)
        {
            if (target.EntityType == Caster.EntityType)
                return;

            foreach (var statusEffect in ExitStatusEffects)
                target.EntityStatComponent.AddStatEffect(statusEffect);
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
    }
}