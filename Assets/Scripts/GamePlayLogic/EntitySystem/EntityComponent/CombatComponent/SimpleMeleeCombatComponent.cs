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
    public class SimpleMeleeCombatComponent : IEntityCombatComponent
    {
        private float _currentAttackRate;
        private bool _canAttack;

        public bool IsInitialization { get; private set; }

        public Stat AttackDamage => Stats[(int)Constant.StatsId.AttackDamage];
        public Stat CritDamage => Stats[(int)Constant.StatsId.CritDamage];
        public Stat CritChance => Stats[(int)Constant.StatsId.CritChance];
        public Stat AttackRate => Stats[(int)Constant.StatsId.AttackRate];
        public Stat AttackRange => Stats[(int)Constant.StatsId.AttackRange];
        
        public event Action OnAttack;

        public BaseGameEntity GameEntity { get; private set; }  
        public Dictionary<int, Stat> Stats { get; private set; }
        
        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }
        
        public void Init(BaseGameEntity parameter1, CombatComponentConfig parameter2)
        {
            Init(parameter1);
            
            Stats = new Dictionary<int, Stat>()
            {
                {(int)Constant.StatsId.AttackDamage,new Stat(Constant.StatsId.AttackDamage,parameter2.AttackDamage)},
                {(int)Constant.StatsId.AttackRate,new Stat(Constant.StatsId.AttackRate,parameter2.AttackRate)},
                {(int)Constant.StatsId.AttackRange,new Stat(Constant.StatsId.AttackRange,1.5f)},
                {(int)Constant.StatsId.CritChance,new Stat(Constant.StatsId.CritChance,parameter2.CritChance)},
                {(int)Constant.StatsId.CritDamage,new Stat(Constant.StatsId.CritDamage,parameter2.CritDamage)},
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
        
        public bool Attack(ITargetAbleEntity targetAbleEntity)
        {
            if(!_canAttack)
                return false;
            
            _canAttack = false;
            
            if (CritChance.CurrentValue > Random.Range(0, 100))
            {
                targetAbleEntity.EntityVisualComponent?.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);//may need to by
                targetAbleEntity.EntityHealthComponent.TakeDamage(AttackDamage.CurrentValue * (CritDamage.CurrentValue / 100),true);
                OnAttack?.Invoke();
                return true;
            }
            
            targetAbleEntity.EntityVisualComponent?.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
            targetAbleEntity.EntityHealthComponent.TakeDamage(AttackDamage.CurrentValue,false);
            OnAttack?.Invoke();
            return true;
        }
    }
}