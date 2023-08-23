using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.SerializeData.AbilitySystemSerializeData;
using UnityEngine;

namespace Tzipory.AbilitiesSystem
{
    public class AbilityHandler
    {
        private IEntityTargetAbleComponent Caster { get; }
        private Dictionary<string, Ability> Abilities { get; }
        
        public bool IsCasting => Abilities.Any(ability => ability.Value.IsCasting);
        
        [Obsolete("Use AbilitySerializeData")]
        public AbilityHandler(IEntityTargetAbleComponent caster,IEntityTargetingComponent entityTargetingComponent,IEnumerable<AbilityConfig> abilitiesConfig)//temp
        {
            Abilities = new Dictionary<string, Ability>();
            Caster = caster;

            foreach (var abilityConfig in abilitiesConfig)
                Abilities.Add(abilityConfig.AbilityName,new Ability(caster,entityTargetingComponent,abilityConfig));
        }

        public void CastAbilityByName(string abilityName,IEnumerable<IEntityTargetAbleComponent> availableTargets)
        {
            if (Abilities.TryGetValue(abilityName, out var ability))
                ability.ExecuteAbility(availableTargets);
            else
                Debug.LogError($"{Caster.EntityTransform.name} cant find ability {abilityName}");
        }

        public void CastAbility(IEnumerable<IEntityTargetAbleComponent> availableTargets)
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