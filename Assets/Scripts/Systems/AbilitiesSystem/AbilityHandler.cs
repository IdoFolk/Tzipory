using System;
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
        public Dictionary<string, Ability> Abilities { get; private set; }
        public Dictionary<int, Stat> Stats { get; private set; }
        public bool IsCasting => Abilities.Any(ability => ability.Value.IsCasting);
        public BaseGameEntity GameEntity { get; private set; }
        
        public bool IsInitialization { get; private set; }
        
        public void Init(BaseGameEntity parameter1, AbilityComponentConfig parameter2)
        {
            Abilities = new Dictionary<string, Ability>();
            _caster = parameter1 as ITargetAbleEntity;
            
            GameEntity = parameter1;

            IEntityTargetingComponent entityTargetingComponent = GameEntity.RequestComponent<IEntityTargetingComponent>();

            foreach (var abilityConfig in parameter2._abilityConfigs)
                Abilities.Add(abilityConfig.AbilityName,new Ability(_caster,entityTargetingComponent,abilityConfig));

            Stats = new Dictionary<int, Stat>();
            
            IsInitialization = true;
        }
        
        public void CastAbilityByName(string abilityName,IEnumerable<ITargetAbleEntity> availableTargets)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
                ability.ExecuteAbility(availableTargets);
            else
                Debug.LogError($"{_caster.GameEntity.name} cant find ability {abilityName}");
        }

        public void CastAbility(IEnumerable<ITargetAbleEntity> availableTargets)//Temp!
        {
            if (Abilities.Count == 0)
                return;
            var ability = Abilities.First().Value;
            ability?.ExecuteAbility(availableTargets);
        }

        public void CancelCast()
        {
            foreach (var abilities in Abilities.Values)
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

            foreach (var abilitiesValue in Abilities.Values)
                statHolders.AddRange(abilitiesValue.GetNestedStatHolders());

            return statHolders;
        }
    }
}