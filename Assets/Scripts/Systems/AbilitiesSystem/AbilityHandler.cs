using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem
{
    public class AbilityHandler
    {
        private ITargetAbleEntity Caster { get; }
        public Dictionary<string, Ability> Abilities { get; }
        
        public bool IsCasting => Abilities.Any(ability => ability.Value.IsCasting);
        
        [Obsolete("Use AbilitySerializeData")]
        public AbilityHandler(ITargetAbleEntity caster,IEntityTargetingComponent entityTargetingComponent,IEnumerable<AbilityConfig> abilitiesConfig)//temp
        {
            Abilities = new Dictionary<string, Ability>();
            Caster = caster;

            foreach (var abilityConfig in abilitiesConfig)
                Abilities.Add(abilityConfig.AbilityName,new Ability(caster,entityTargetingComponent,abilityConfig));
        }

        public void CastAbilityByName(string abilityName,IEnumerable<ITargetAbleEntity> availableTargets)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
                ability.ExecuteAbility(availableTargets);
            else
                Debug.LogError($"{Caster.EntityTransform.name} cant find ability {abilityName}");
        }

        public void CastAbility(IEnumerable<ITargetAbleEntity> availableTargets)
        {
            if (Abilities.Count == 0)
                return;
            Abilities.First().Value?.ExecuteAbility(availableTargets);
        }

        public void CancelCast()
        {
            foreach (var abilities in Abilities.Values)
            {
                if (abilities.IsCasting)
                    abilities.CancelCast();
            }
        }
    }
}