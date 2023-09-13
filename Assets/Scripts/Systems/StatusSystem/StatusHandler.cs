using System;
using System.Collections.Generic;
using Helpers.Consts;
using Tzipory.GameplayLogic.StatusEffectTypes;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.GameplayLogic.StatusEffectTypes
{
    public class StatusHandler
    {
        public event Action<EffectSequenceConfig> OnStatusEffectAdded; 
        public event Action<int> OnStatusEffectInterrupt; 

        private readonly IEntityStatusEffectComponent _entity;

        private readonly List<IStatHolder> _statHolders;

#if UNITY_EDITOR
        [Obsolete("Only in editor")]
        public List<IStatHolder> StatHolders => _statHolders;
#endif

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

        public Stat GetStat(int id)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue(id, out Stat stat))
                    return stat;
            }

            Debug.LogError($"Stat ID: {id} not found in StatusHandler of entity {_entity.GameEntity.name}");
            return  null;
        }
        
        public Stat GetStat(Constant.StatsId statIdToFind)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue((int)statIdToFind, out Stat stat))
                    return stat;
            }

            Debug.LogError($"Stat ID: {statIdToFind} not found in StatusHandler of entity {_entity.GameEntity.name}");
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
            var statToEffect = GetStat(statusEffectConfig.AffectedStatId);
            
            //   TODO need to Interrupt stats

            OnStatusEffectAdded?.Invoke(statusEffectConfig.EffectSequence);
            
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