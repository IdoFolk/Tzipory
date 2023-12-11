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
    public class AoeInstantiateExecuter : IAbilityExecutor , IStatHolder , IInitialization<ITargetAbleEntity,ExecuterConfig,IAbilityExecutor>
    {
        private const string AOE_PREFAB_PATH = "";

        private GameObject _gameObject;
        
        private IAbilityExecutor _abilityExecutor;
        
        private ITargetAbleEntity _caster;

        public Dictionary<int, Stat> Stats { get; private set; }

        public bool IsInitialization { get; private set; }
        public void Init(ITargetAbleEntity parameter1, ExecuterConfig parameter2,IAbilityExecutor abilityExecutor)
        {
            _caster = parameter1;
            
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
            projectilePrefab.GetComponent<AoeAbilityEntity>().Init(_caster,target.GameEntity.transform.position,_abilityExecutor);
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            return new IStatHolder[] { this };
        }
    }
}