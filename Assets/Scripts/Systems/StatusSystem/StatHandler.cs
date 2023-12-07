﻿using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem
{
    public class StatHandler
    {
        public event Action<EffectSequenceConfig> OnStatusEffectAdded; 

        private readonly IEntityStatComponent _entity;

        private readonly List<IStatHolder> _statHolders;
        
        private readonly Dictionary<Constant.StatHolderType,List<IStatHolder>> _statHolderByType = new();

#if UNITY_EDITOR
        [Obsolete("Only in editor")]
        public List<IStatHolder> StatHolders => _statHolders;
#endif

        public StatHandler(IEntityStatComponent entity)
        {
            _statHolders = new List<IStatHolder>();
            
            _entity = entity;
            
            AddStatHolder(_entity.GetNestedStatHolders());
        }

        private void AddStatHolder(IEnumerable<IStatHolder> statHolders)
        {
            _statHolders.AddRange(statHolders);  
        }

        public Stat GetStat(int id)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue(id, out Stat stat))
                    return stat;
            }

            Debug.LogError($"Stat ID: {id} not found in StatHandler of entity {_entity.GameEntity.name}");
            return  null;
        }
        
        public Stat GetStat(Constant.StatsId statIdToFind)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue((int)statIdToFind, out Stat stat))
                    return stat;
            }

            Debug.LogError($"Stat ID: {statIdToFind} not found in StatHandler of entity {_entity.GameEntity.name}");
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
        
        public IStatEffectProcess AddStatEffect(StatEffectConfig statEffectConfig)
        {
            var statToEffect = GetStat(statEffectConfig.AffectedStatId);
            
            var statusEffect =  FactorySystem.ObjectFactory.StatusEffectFactory.GetStatusEffect(statEffectConfig,statToEffect);
            //   TODO need to Interrupt stats

            OnStatusEffectAdded?.Invoke(statEffectConfig.EffectSequence);
            
            if (statEffectConfig.OverrideGlobalPopUpTextConfig)
                return statToEffect.AddStatusEffect(statusEffect,statEffectConfig.PopUpTextConfig);
            
            return statToEffect.AddStatusEffect(statusEffect);

        }

        //TODO need to fix the InterruptStatusEffects
        // private void InterruptStatusEffects(IEnumerable<StatEffectConfigs> effectConfigSos)
        // {
        //     foreach (var effectConfigSo in effectConfigSos)
        //     {
        //         if (_activeStatusEffects.TryGetValue(effectConfigSo.AffectedStatId,out var modifyStatEffect))
        //         {
        //             modifyStatEffect.StatusEffectInterrupt();
        //             _activeStatusEffects.Remove(effectConfigSo.AffectedStatId);
        //             OnStatusEffectInterrupt?.Invoke(modifyStatEffect.AffectedStatId);
        //         }
        //     }
        // }
    }
}