using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.StatSystemSerializeData;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Debag;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class StandardHealthComponent : IEntityHealthComponent
    {
        public Action<bool> OnHit { get; }
        public Action<Action> OnDeath { get; }

        private float _currentInvincibleTime;
        
        private bool _startedDeathSequence;
        
        public BaseGameEntity GameEntity { get; private set;  }

        public Stat InvincibleTime => Stats[(int) Constant.StatsId.InvincibleTime];
        public Stat Health => Stats[(int) Constant.StatsId.Health];
        
        public Dictionary<int, Stat> Stats { get; private set; }
        
        public bool IsDamageable { get; private set; }
        public bool IsEntityDead => Health.CurrentValue <= 0;
        
        public bool IsInitialization { get; private set; }
        
        public void Init(BaseGameEntity baseGameEntity)
        {
            GameEntity = baseGameEntity;
        }
        
        public void Init(BaseGameEntity baseGameEntity, HealthComponentConfig config)
        {
            Init(baseGameEntity);

            Stats = new Dictionary<int, Stat>()
            {
                {(int)Constant.StatsId.Health, new Stat(Constant.StatsId.Health,config.HealthStat)},
                {(int)Constant.StatsId.InvincibleTime, new Stat(Constant.StatsId.InvincibleTime,config.InvincibleTimeStat)},
            };
            
            IsDamageable = true;
            
            IsInitialization = true;
        }
        
        public void UpdateComponent()
        {
            if (IsDamageable) return;
            
            _currentInvincibleTime -= GAME_TIME.GameDeltaTime;

            if (_currentInvincibleTime < 0)
            {
                IsDamageable = true;
                _currentInvincibleTime = InvincibleTime.CurrentValue;
            }
        }

        public void Heal(float amount)
        {
            Health.ProcessStatModifier(new StatModifier(amount,StatusModifierType.Addition),"Heal",PopUpTextManager.Instance.HealDefaultConfig);
        }

        public void TakeDamage(float damage, bool isCrit)
        {
            if (!IsDamageable) return;
            
            IsDamageable = false; // Is this what turns on InvincibleTime?

            PopUpTextConfig popUpTextConfig;
            string processName;

            if (isCrit)
            {
                popUpTextConfig = PopUpTextManager.Instance.GetCritHitDefaultConfig;
                processName = "Crit Hit";
            }
            else
            {
                popUpTextConfig = PopUpTextManager.Instance.GetHitDefaultConfig;
                processName = "Hit";
            }

            Health.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce),processName,popUpTextConfig);
            IsDamageable = false;
        }

        public void StartDeathSequence()
        {
            _startedDeathSequence = true;
            
            Logger.Log($"<color={ColorLogHelper.ENTITY_COLOR}>{GameEntity.name}</color> as started death sequence",BaseGameEntity.ENTITY_LOG_GROUP);
            
            IsDamageable = false;
            
            OnDeath?.Invoke(EntityDied);
        }
        
        private void EntityDied()
        {
            IsInitialization = false;
            Logger.Log($"<color={ColorLogHelper.ENTITY_COLOR}>{GameEntity.name}</color> as died!",BaseGameEntity.ENTITY_LOG_GROUP);
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            return new List<IStatHolder> { this };
        }
    }
}