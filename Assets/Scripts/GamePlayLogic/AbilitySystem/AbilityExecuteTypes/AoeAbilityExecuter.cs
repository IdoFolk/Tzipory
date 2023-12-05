using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.AbilitySystem.AbilityEntity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class AoeAbilityExecuter :  IAbilityExecutor , IStatHolder , IInitialization<ITargetAbleEntity>
    {
        private const string  AOE_PREFAB_PATH = "Prefabs/Ability/AoeAbilityEntity";
        
        private readonly GameObject _aoePrefab;

        private readonly AbilityConfig _abilityConfig;
        
        private List<BaseModifyStatEffect> _statusEffects;
        public AbilityExecuteType AbilityExecuteType => AbilityExecuteType.AOE;
        public ITargetAbleEntity Caster { get; }

        //Changes:
        public List<StatEffectConfig> EnterStatusEffects { get; }
        public List<StatEffectConfig> ExitStatusEffects { get; }
        
        public Dictionary<int, Stat> Stats { get; }
        
        [Obsolete("Use AbilitySerializeData")]
        public AoeAbilityExecuter(ITargetAbleEntity caster,AbilityConfig abilityConfig)
        {
            Stats = new Dictionary<int, Stat>();

            Caster = caster;
           
            EnterStatusEffects = new List<StatEffectConfig>();
            ExitStatusEffects = new List<StatEffectConfig>();

            _abilityConfig = abilityConfig;
            
            EnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
            if(abilityConfig.DoExitEffects)
                ExitStatusEffects.AddRange(abilityConfig.ExitStatusEffectConfigs);
            
            Stats.Add((int)Constant.StatsId.AoeRadius,new Stat("AoeRadius", abilityConfig.AoeRadius, int.MaxValue, (int)Constant.StatsId.AoeRadius));
            Stats.Add((int)Constant.StatsId.AoeDuration, new Stat("AoeDuration", abilityConfig.AoeDuration, int.MaxValue, (int)Constant.StatsId.AoeDuration));
            
            _aoePrefab = Resources.Load<GameObject>(AOE_PREFAB_PATH);
            IsInitialization = false;
        }

        public bool IsInitialization { get; private set; }

        public void Init(ITargetAbleEntity target)//temp
        {
            var aoeGameObject = Object.Instantiate(_aoePrefab).GetComponent<AoeAbilityEntity>();
            //aoeGameObject.Init(this,_abilityConfig,Stats); //Here the settings need to be changed
            Execute(target);
            IsInitialization = true;
        }

        public void Execute(ITargetAbleEntity target)
        {
            if (target.EntityType == Caster.EntityType)
                return;

            foreach (var statusEffect in EnterStatusEffects)
                target.EntityStatComponent.AddStatEffect(statusEffect);
            if (_abilityConfig.AbilityVisualConfig.HaveEffectOnEntity)
                target.EntityVisualComponent?.StartAnimationEffect(_abilityConfig.AbilityVisualConfig.TargetAnimationConfig);
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