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
    public class AoeInstantiateExecuter : IAbilityExecutor , IStatHolder , IInitialization<ITargetAbleEntity,ExecuterConfig,IAbilityExecutor,AbilityVisualConfig>
    {
        private const string AOE_PREFAB_PATH = "Prefabs/Ability/AoeAbilityEntity";

        private GameObject _gameObject;
        
        private IAbilityExecutor _abilityExecutor;
        
        private ITargetAbleEntity _caster;

        private AbilityVisualConfig _abilityVisualConfig;

        public Dictionary<int, Stat> Stats { get; private set; }

        public bool IsInitialization { get; private set; }
        public void Init(ITargetAbleEntity parameter1, ExecuterConfig parameter2,IAbilityExecutor abilityExecutor,AbilityVisualConfig abilityVisualConfig)
        {
            _caster = parameter1;
            
            _abilityVisualConfig = abilityVisualConfig;
            
            Stats = new Dictionary<int, Stat>
            {
                {
                    (int)Constant.StatsId.AoeDuration, new Stat("AoeDuration", parameter2.AoeDuration,
                        int.MaxValue,
                        (int)Constant.StatsId.AoeDuration)
                },
                {
                    (int)Constant.StatsId.AoeRadius, new Stat("AoeRadius",
                        parameter2.AoeRadius, int.MaxValue,
                        (int)Constant.StatsId.AoeRadius)
                }
            };
            
            _gameObject = Resources.Load<GameObject>(AOE_PREFAB_PATH);
            
            if (_gameObject is null)
                throw new Exception($"{nameof(ProjectileInstantiateExecuter)} ProjectilePrefab not found");

            _abilityExecutor = abilityExecutor;
            
            IsInitialization = true;
        }
        
        public void Execute(ITargetAbleEntity target)
        {
            var projectilePrefab = Object.Instantiate(_gameObject,target.GameEntity.transform.position,Quaternion.identity);
            projectilePrefab.GetComponent<AoeAbilityEntity>().Init(_caster,target.GameEntity.transform.position,_abilityExecutor,_abilityVisualConfig);
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders = new List<IStatHolder>();
            statHolders.Add(this);

            if (_abilityExecutor is IStatHolder statHolder)
            {
                statHolders.Add(statHolder);
            }

            return statHolders;
        }
    }
}