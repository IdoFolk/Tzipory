using System;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    public class StatusHandler
    {
        public event Action<BaseStatusEffect> OnStatusEffectAdded; 
        public event Action<int> OnStatusEffectInterrupt; 

        private readonly IEntityStatusEffectComponent _entity;

        private readonly List<IStatHolder> _statHolders;//need to make a big refactor! for this to work
        
        public StatusHandler(IEntityStatusEffectComponent entity)
        {
            _statHolders = new List<IStatHolder>();
            
            _entity = entity;
            
            _statHolders.AddRange(_entity.GetNestedStatHolders());
        }

        public void AddStatHolder(IStatHolder statHolder)
        {
            _statHolders.Add(statHolder);
        }

        public Stat GetStatById(int id)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue(id, out Stat stat))
                    return stat;
            }

            Debug.LogError($"Stat ID: {id} not found in StatusHandler of entity {_entity.GameEntity.name}");
            return  null;
        }

        public void UpdateStatHandler()
        {
            foreach (var statHolder in _statHolders)
            {
                foreach (var stat in statHolder.Stats.Values)
                    stat.UpdateStatusEffects();
            }
        }
        
        public IDisposable AddStatusEffect(StatusEffectConfig statusEffectConfig)
        {
            var statToEffect = GetStatById(statusEffectConfig.AffectedStatId);
            
            //   TODO need to Interrupt stats
            
            return statToEffect.AddStatusEffect(statusEffectConfig);
        }
            
        //TODO need to fix the InterruptStatusEffects
        // private void InterruptStatusEffects(IEnumerable<StatusEffectConfig> effectConfigSos)
        // {
        //     foreach (var effectConfigSo in effectConfigSos)
        //     {
        //         if (_activeStatusEffects.TryGetValue(effectConfigSo.AffectedStatId,out var statusEffect))
        //         {
        //             statusEffect.StatusEffectInterrupt();
        //             _activeStatusEffects.Remove(effectConfigSo.AffectedStatId);
        //             OnStatusEffectInterrupt?.Invoke(statusEffect.AffectedStatId);
        //         }
        //     }
        // }
    }
}