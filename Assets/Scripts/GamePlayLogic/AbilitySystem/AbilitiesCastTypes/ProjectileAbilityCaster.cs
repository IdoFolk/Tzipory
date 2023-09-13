using System;
using System.Collections.Generic;
using Helpers.Consts;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.GameplayLogic.StatusEffectTypes;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.GameplayLogic.AbilitySystem.AbilityEntity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.GameplayLogic.AbilitySystem
{
    public class ProjectileAbilityCaster : IAbilityCaster , IStatHolder
    {
        private const string ProjectilePrefabPath = "Prefabs/Ability/ProjectileAbilityEntity";

        public event Action OnCast;
       //public AbilityCastType AbilityCastType { get; }
        
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
            
            Stats = new Dictionary<int, Stat>();
            
            Stats.Add((int)Constant.StatsId.ProjectileSpeed,  new Stat("ProjectileSpeed", config.ProjectileSpeed, int.MaxValue,
                (int)Constant.StatsId.ProjectileSpeed));
            Stats.Add((int)Constant.StatsId.ProjectilePenetration, new Stat("ProjectilePenetration", config.ProjectilePenetration, int.MaxValue,
                (int)Constant.StatsId.ProjectilePenetration));
            
            _projectilePrefab = Resources.Load<GameObject>(ProjectilePrefabPath);

            if (_projectilePrefab is null)
                throw  new System.Exception($"{nameof(ProjectileAbilityCaster)} ProjectilePrefab not found");
        }

        public void Cast(IEntityTargetAbleComponent target, IAbilityExecutor abilityExecutor)
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