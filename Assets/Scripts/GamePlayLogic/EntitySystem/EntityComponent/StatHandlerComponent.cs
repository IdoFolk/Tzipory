using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.FactorySystem.ObjectFactory;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Debag;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class StatHandlerComponent : IEntityStatComponent
    {
        #region Events

        public event Action<EffectSequenceConfig> OnStatusEffectAdded; 

        #endregion
        
        #region Proprty

        public bool IsInitialization { get; private set; }
        
        public BaseGameEntity GameEntity { get; private set; }
        
        private List<IStatHolder> _statHolders;

        #endregion

        #region Init

        public void Init(BaseGameEntity baseGameEntity, IEnumerable<IStatHolder> statHolders)
        {
            Init(baseGameEntity);
            
            _statHolders = new List<IStatHolder>();
            
            _statHolders.AddRange(statHolders);  
            
            IsInitialization = true;
        }
        

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }


        #endregion

        #region PublicMethod

        public void UpdateComponent()
        {
            foreach (var statHolder in _statHolders)
            {
                foreach (var stat in statHolder.Stats.Values)
                    stat.UpdateStatusEffects();
            }
        }
        
        public Stat GetStat(int id)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue(id, out Stat stat))
                    return stat;
            }

            Logger.LogError($"Stat ID: {id} not found in StatHandler of entity {GameEntity.name}");
            return  null;
        }

        public IEnumerable<Stat> GetAllStats()
        {
            foreach (var statHolder in _statHolders)
            {
                foreach (var statsValue in statHolder.Stats.Values)
                    yield return statsValue;
            }
        }

        public Stat GetStat(Constant.StatsId statIdToFind)
        {
            foreach (var statHolder in _statHolders)
            {
                if (statHolder.Stats.TryGetValue((int)statIdToFind, out Stat stat))
                    return stat;
            }

            Logger.LogError($"Stat ID: {statIdToFind} not found in StatHandler of entity {GameEntity.name}");
            return  null;
        }
        
        public IDisposable AddStatEffect(StatEffectConfig statEffectConfig)
        {
            var statToEffect = GetStat(statEffectConfig.AffectedStatId);
            var statusEffect =  StatusEffectFactory.GetStatusEffect(statEffectConfig,statToEffect);
            //   TODO need to Interrupt stats

            OnStatusEffectAdded?.Invoke(statEffectConfig.EffectSequence);
            
            if (statEffectConfig.OverrideGlobalPopUpTextConfig)
                return statToEffect.AddStatusEffect(statusEffect,statEffectConfig.PopUpTextConfig);
            
            return statToEffect.AddStatusEffect(statusEffect);
        }

        #endregion
    }
}