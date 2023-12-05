using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Systems.AbilitySystem
{
    public class InstantAbilityCaster : IAbilityCaster
    {
        public event Action OnCast;
        
        private readonly AbilityConfig _config;

        private readonly GameObject _abilityEntity;
        
        public InstantAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent,AbilityConfig config)
        {
            _config  = config;
            _abilityEntity = Resources.Load<GameObject>(config.AbilityPrefabPath);
        }
        
        public void Cast(ITargetAbleEntity target, IAbilityExecutor abilityExecutor)
        {
            // var abilityEntity = Object.Instantiate(_abilityEntity, target.GameEntity.EntityTransform.position, Quaternion.identity);
            // abilityEntity.GetComponent<BaseAbilityEntity>().Init(abilityExecutor,_config,null);
            OnCast?.Invoke();
            abilityExecutor.Init(target);//may need to remove!
        }
    }
}