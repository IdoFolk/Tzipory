using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.TimeSystem;
using Random = UnityEngine.Random;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class SimpleRangeCombatComponent : IEntityCombatComponent
    {
        private Temp_ShotVisual _shotVisual;
        private float _currentAttackRate;
        private bool _canAttack;
        private IEntityVisualComponent _entityVisualComponent;

        public bool IsInitialization { get; private set; }

        public Stat AttackDamage => Stats[(int)Constant.StatsId.AttackDamage];
        public Stat CritDamage => Stats[(int)Constant.StatsId.CritDamage];
        public Stat CritChance => Stats[(int)Constant.StatsId.CritChance];
        public Stat AttackRate => Stats[(int)Constant.StatsId.AttackRate];
        public Stat AttackRange => Stats[(int)Constant.StatsId.AttackRange];
        
        public BaseGameEntity GameEntity { get; private set; }  
        public Dictionary<int, Stat> Stats { get; private set; }
        
        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }
        
        public void Init(BaseGameEntity baseGameEntity, CombatComponentConfig config)
        {
            Init(baseGameEntity);
            
            _shotVisual = GameEntity.GetComponentInChildren<Temp_ShotVisual>();//temp may need to change
            
            _entityVisualComponent = baseGameEntity.RequestComponent<IEntityVisualComponent>();
            
            if (_shotVisual is null)
                throw new Exception($"Can not find Temp_ShotVisual in {GameEntity.name}");
            
            _shotVisual.Init(baseGameEntity,config.ProjectilePrefab,config.ProjectileSpeed,config.ProjectileTimeToDie);

            Stats = new Dictionary<int, Stat>()
            {
                {(int)Constant.StatsId.AttackDamage,new Stat(Constant.StatsId.AttackDamage,config.AttackDamage)},
                {(int)Constant.StatsId.AttackRate,new Stat(Constant.StatsId.AttackRate,config.AttackRate)},
                {(int)Constant.StatsId.AttackRange,new Stat(Constant.StatsId.AttackRange,config.AttackRange)},
                {(int)Constant.StatsId.CritChance,new Stat(Constant.StatsId.CritChance,config.CritChance)},
                {(int)Constant.StatsId.CritDamage,new Stat(Constant.StatsId.CritDamage,config.CritDamage)},
            };
            
            IsInitialization = true;
        }

        public void UpdateComponent()
        {
            if (_canAttack)
                return;
            
            _currentAttackRate -= GAME_TIME.GameDeltaTime;
            
            if (_currentAttackRate < 0)
            {
                _currentAttackRate = AttackRate.CurrentValue;
                _canAttack = true;
            }
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            return new IStatHolder[] { this };
        }
        
        public void Attack(ITargetAbleEntity targetAbleEntity)
        {
            if(!_canAttack)
                return;
            
            _canAttack = false;
            
            if (CritChance.CurrentValue > Random.Range(0, 100))
            {
                _entityVisualComponent.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);
                _shotVisual.Shot(targetAbleEntity,AttackDamage.CurrentValue * (CritDamage.CurrentValue / 100),true);
                return;
            }
            
            _entityVisualComponent.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
            _shotVisual.Shot(targetAbleEntity,AttackDamage.CurrentValue,false);
        }
    }
}