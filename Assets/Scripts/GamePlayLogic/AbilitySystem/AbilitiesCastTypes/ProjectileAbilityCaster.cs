using System;
using Helpers.Consts;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.AbilitiesSystem.AbilityEntity;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.AbilitiesSystem
{
    public class ProjectileAbilityCaster : IAbilityCaster
    {
        private const string ProjectilePrefabPath = "Prefabs/Ability/ProjectileAbilityEntity";

        public event Action OnCast;
       //public AbilityCastType AbilityCastType { get; }
        
        public IEntityTargetingComponent EntityCasterTargetingComponent { get; }

        private Stat ProjectileSpeed { get;}
        private Stat ProjectilePenetration { get;}

        private readonly GameObject _projectilePrefab;

        public ProjectileAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent, AbilityConfig config)
        {
            EntityCasterTargetingComponent = entityCasterTargetingComponent;

            ProjectileSpeed = new Stat("ProjectileSpeed", config.ProjectileSpeed, int.MaxValue,
                (int)Constant.Stats.ProjectileSpeed);
            
            ProjectilePenetration = new Stat("ProjectilePenetration", config.ProjectilePenetration, int.MaxValue,
                (int)Constant.Stats.ProjectilePenetration);

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
    }
}