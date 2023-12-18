using System;
using System.Collections.Generic;
using GamePlayLogic.AbilitySystem.AbilityEntity;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class ProjectileInstantiateExecuter : IAbilityExecutor , IStatHolder , IInitialization<ITargetAbleEntity,ExecuterConfig,IAbilityExecutor,AbilityVisualConfig>
    {
        private const string PROJECTILE_PREFAB_PATH = "Prefabs/Ability/AbilityEntity/ProjectileAbilityEntity";
        
        private GameObject _projectilePrefab;

        private ITargetAbleEntity _caster;

        private IAbilityExecutor _abilityExecutor;
        
        private AbilityVisualConfig  _abilityVisualConfig;
        
        public Dictionary<int, Stat> Stats { get; private set; }
        public bool IsInitialization { get; private set; }
        
        public void Init(ITargetAbleEntity caster,ExecuterConfig parameter,IAbilityExecutor abilityExecutor,AbilityVisualConfig abilityVisualConfig)
        {
            _caster = caster;
            
            _abilityVisualConfig = abilityVisualConfig;
            
            Stats = new Dictionary<int, Stat>
            {
                {
                    (int)Constant.StatsId.ProjectileSpeed, new Stat("ProjectileSpeed", parameter.ProjectileSpeed,
                        int.MaxValue,
                        (int)Constant.StatsId.ProjectileSpeed)
                },
                {
                    (int)Constant.StatsId.ProjectilePenetration, new Stat("ProjectilePenetration",
                        parameter.ProjectilePenetration, int.MaxValue,
                        (int)Constant.StatsId.ProjectilePenetration)
                }
            };

            _projectilePrefab = Resources.Load<GameObject>(PROJECTILE_PREFAB_PATH);
            
            if (_projectilePrefab is null)
                throw new Exception($"{nameof(ProjectileInstantiateExecuter)} ProjectilePrefab not found");

            _abilityExecutor = abilityExecutor;
            
            IsInitialization = true;
        }
        
        public void Execute(ITargetAbleEntity target)
        {
            var projectilePrefab = Object.Instantiate(_projectilePrefab,_caster.GameEntity.transform.position,Quaternion.identity);
            projectilePrefab.GetComponent<ProjectileAbilityEntity>().Init(_caster,target.GameEntity.transform.position,_abilityExecutor,_abilityVisualConfig);
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders = new List<IStatHolder> {this};

            if (_abilityExecutor is IStatHolder statHolder)
                statHolders.Add(statHolder);

            return statHolders;
        }
    }
}