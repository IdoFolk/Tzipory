using System.Collections.Generic;
using System.Linq;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem
{
    public class AbilityHandler : IEntityAbilitiesComponent
    {
        private ITargetAbleEntity _caster;
        private Dictionary<string, Ability> _abilities;
        public Dictionary<int, Stat> Stats { get; private set; }
        public bool IsCasting => _abilities.Any(ability => ability.Value.IsCasting);
        public BaseGameEntity GameEntity { get; private set; }
        
        public bool IsInitialization { get; private set; }
        
        public void Init(BaseGameEntity parameter1, AbilityComponentConfig parameter2)
        {
            _abilities = new Dictionary<string, Ability>();
            _caster = parameter1 as ITargetAbleEntity;
            
            GameEntity = parameter1;

            IEntityTargetingComponent entityTargetingComponent = GameEntity.RequestComponent<IEntityTargetingComponent>();

            foreach (var abilityConfig in parameter2._abilityConfigs)
                _abilities.Add(abilityConfig.AbilityName,new Ability(_caster,entityTargetingComponent,abilityConfig));

            Stats = new Dictionary<int, Stat>();
            
            IsInitialization = true;
        }
        
        public void CastAbilityByName(string abilityName,IEnumerable<ITargetAbleEntity> availableTargets)
        {
            if (_abilities.TryGetValue(abilityName, out var ability))
                ability.ExecuteAbility(availableTargets);
            else
                Debug.LogError($"{_caster.GameEntity.name} cant find ability {abilityName}");
        }

        public void CastAbility(IEnumerable<ITargetAbleEntity> availableTargets)//Temp!
        {
            if (_abilities.Count == 0)
                return;
            _abilities.First().Value?.ExecuteAbility(availableTargets);
        }

        public void CancelCast()
        {
            foreach (var abilities in _abilities.Values)
            {
                if (abilities.IsCasting)
                    abilities.CancelCast();
            }
        }
        
        public void UpdateComponent()
        {
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders  = new List<IStatHolder>();

            foreach (var abilitiesValue in _abilities.Values)
                statHolders.AddRange(abilitiesValue.GetNestedStatHolders());

            return statHolders;
        }
    }
}