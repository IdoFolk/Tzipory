using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.AbilitySystem.AbilityEntity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem
{
    public class ProjectileAbilityCaster : IAbilityCaster , IStatHolder
    {
        public event Action OnCast;

        private const string PROJECTILE_PREFAB_PATH = "Prefabs/Ability/ProjectileAbilityEntity";
        
        public IEntityTargetingComponent EntityCasterTargetingComponent { get; }
        
        public Dictionary<int, Stat> Stats { get; }

        private Stat ProjectileSpeed   
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.ProjectileSpeed, out var projectileSpeed))
                    return projectileSpeed;
                
                throw new Exception($"ProjectileSpeed not found in entity {EntityCasterTargetingComponent.GameEntity.name}");
            }
        }
        
        private Stat ProjectilePenetration 
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.ProjectilePenetration, out var projectilePenetration))
                    return projectilePenetration;
                
                throw new Exception($"CastTime not found in entity {EntityCasterTargetingComponent.GameEntity.name}");
            }
        }

        private readonly GameObject _projectilePrefab;

        public ProjectileAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent, AbilityConfig config)
        {
            EntityCasterTargetingComponent = entityCasterTargetingComponent;
            
            Stats = new Dictionary<int, Stat>
            {
                {
                    (int)Constant.StatsId.ProjectileSpeed, new Stat("ProjectileSpeed", config.ProjectileSpeed,
                        int.MaxValue,
                        (int)Constant.StatsId.ProjectileSpeed)
                },
                {
                    (int)Constant.StatsId.ProjectilePenetration, new Stat("ProjectilePenetration",
                        config.ProjectilePenetration, int.MaxValue,
                        (int)Constant.StatsId.ProjectilePenetration)
                }
            };

            _projectilePrefab = Resources.Load<GameObject>(PROJECTILE_PREFAB_PATH);

            if (_projectilePrefab is null)
                throw  new Exception($"{nameof(ProjectileAbilityCaster)} ProjectilePrefab not found");
        }

        public void Cast(ITargetAbleEntity target, IAbilityExecutor abilityExecutor)
        {
            OnCast?.Invoke();
            var projectilePrefab = Object.Instantiate(_projectilePrefab,EntityCasterTargetingComponent.ShotPosition,Quaternion.identity);
            projectilePrefab.GetComponent<ProjectileAbilityEntity>().Init(target,ProjectileSpeed.CurrentValue,ProjectilePenetration.CurrentValue,abilityExecutor);
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
    }
}