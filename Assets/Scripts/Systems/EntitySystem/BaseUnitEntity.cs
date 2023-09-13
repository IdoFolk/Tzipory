﻿using System;
using System.Collections.Generic;
using Helpers;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.AbilitySystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig.EntityVisualConfig;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.ConfigFiles.WaveSystemConfig;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using Tzipory.GameplayLogic.StatusEffectTypes;
using UnityEngine;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig;

namespace Tzipory.EntitySystem.Entitys
{
    public abstract class BaseUnitEntity : BaseGameEntity, IEntityTargetAbleComponent, IEntityCombatComponent, IEntityMovementComponent, 
        IEntityTargetingComponent, IEntityAbilitiesComponent, IEntityVisualComponent, IInitialization<BaseUnitEntityConfig> , IInitialization<UnitEntitySerializeData,BaseUnitEntityVisualConfig>
    {
        #region Fields

#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("StatsId")] private List<Stat> _stats;
#endif
        [SerializeField,TabGroup("Component")] private Transform _particleEffectPosition;
        [SerializeField,TabGroup("Component")] private SoundHandler _soundHandler;
        [SerializeField,TabGroup("Component")] private TargetingHandler _targetingHandler;
        [Header("Visual components")]
        [SerializeField,TabGroup("Component")] private SpriteRenderer _spriteRenderer;
        [SerializeField,TabGroup("Component")] private Transform _visualQueueEffectPosition;

        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onDeath;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onCritAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onMove;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onGetHit;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onGetCritHit;

        [SerializeField, TabGroup("Pop-Up Texter")] private PopUpTexter _popUpTexter;
        [SerializeField, TabGroup("Pop-Up Texter")] private PopUpText_Config _defaultPopUpText_Config;
        [SerializeField, TabGroup("Pop-Up Texter")] private PopUpText_Config _critPopUpText_Config;
        [SerializeField, TabGroup("Pop-Up Texter")] private PopUpText_Config _healPopUpText_Config;
        
        private float  _currentInvincibleTime;

        private bool _startedDeathSequence;

        #endregion

        #region Proprty

        public Dictionary<int, Stat> Stats { get; private set; }
        
        public StatusHandler StatusHandler { get; private set; }
        
        public AbilityHandler AbilityHandler { get; private set; }
        
        public EffectSequenceHandler EffectSequenceHandler { get; private set; }
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public SoundHandler SoundHandler => _soundHandler;
        public Transform ParticleEffectPosition => _particleEffectPosition;
        public Transform VisualQueueEffectPosition => _visualQueueEffectPosition;
        public PopUpTexter PopUpTexter => _popUpTexter;
        
        public bool IsDamageable { get; private set; }
        public bool IsEntityDead => Health.CurrentValue <= 0;
        
        //need to remove this and only use the Dictionary!!!
        #region StatsId

         public Stat Health  
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.Health, out var health))
                    return health;
                
                throw new Exception($"{Constant.StatsId.Health} not found in entity {GameEntity.name}");
            }
        }
        public Stat InvincibleTime  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.InvincibleTime, out var invincibleTime))
                    return invincibleTime;
                
                throw new Exception($"{Constant.StatsId.InvincibleTime} not found in entity {GameEntity.name}");
            }
        }
        public Stat AttackDamage  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AttackDamage, out var attackDamage))
                    return attackDamage;
                
                throw new Exception($"{Constant.StatsId.AttackDamage} not found in entity {GameEntity.name}");
            }
        }
        public Stat CritDamage  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.CritDamage, out var critDamage))
                    return critDamage;
                
                throw new Exception($"{Constant.StatsId.CritDamage} not found in entity {GameEntity.name}");
            }
        }
        public Stat CritChance {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.CritChance, out var critChance))
                    return critChance;
                
                throw new Exception($"{Constant.StatsId.CritChance} not found in entity {GameEntity.name}");
            }
        }
        public Stat AttackRate {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AttackRate, out var attackRate))
                    return attackRate;
                
                throw new Exception($"{Constant.StatsId.AttackRate} not found in entity {GameEntity.name}");
            }
        }
        public Stat AttackRange  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AttackRange, out var attackRange))
                    return attackRange;
                
                throw new Exception($"{Constant.StatsId.AttackRange} not found in entity {GameEntity.name}");
            }
        }
        public Stat MovementSpeed  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.MovementSpeed, out var movementSpeed))
                    return movementSpeed;
                
                throw new Exception($"{Constant.StatsId.MovementSpeed} not found in entity {GameEntity.name}");
            }
        }
        public Stat TargetingRange {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.TargetingRange, out var targetingRange))
                    return targetingRange;
                
                throw new Exception($"{Constant.StatsId.TargetingRange} not found in entity {GameEntity.name}");
            }
        }

        #endregion
        
        public event Action<IEntityTargetAbleComponent> OnTargetDisable;
        public bool IsTargetAble { get; private set; }//not in use!
        
        public EntityType EntityType { get; protected set; }
        public Vector2 ShotPosition => _shotPosition.position;
        public IPriorityTargeting DefaultPriorityTargeting { get; private set; }
        public TargetingHandler TargetingHandler => _targetingHandler;
        
        public bool IsInitialization { get; private set; }
        
        #endregion

        #region Temps
        
        [Header("TEMPS")] [SerializeField]
        private Transform _shotPosition;
        [SerializeField] private bool _doShowHPBar;
        [SerializeField] private TEMP_UNIT_HPBarConnector _hpBarConnector;
        
        #endregion
        
        #region Init

        private void BaseInit()
        {
            StatusHandler = new StatusHandler(this);//may need to work in init!

#if UNITY_EDITOR
            foreach (var statHolder in StatusHandler.StatHolders)
                _stats.AddRange(statHolder.Stats.Values);
#endif
            
            _onDeath.ID = Constant.EffectSequenceIds.DEATH;
            _onAttack.ID = Constant.EffectSequenceIds.ATTACK;
            _onCritAttack.ID = Constant.EffectSequenceIds.CRIT_ATTACK;
            _onMove.ID = Constant.EffectSequenceIds.MOVE;
            _onGetHit.ID = Constant.EffectSequenceIds.GET_HIT;
            _onGetCritHit.ID = Constant.EffectSequenceIds.GET_CRIT_HIT;
            
            _onDeath.SequenceName = "OnDeath";
            _onAttack.SequenceName = "OnAttack";
            _onCritAttack.SequenceName = "OnCritAttack";
            _onMove.SequenceName = "OnMove";
            _onGetHit.SequenceName = "OnGetHit";
            _onGetCritHit.SequenceName = "OnGetCritHit";
            
            var effectSequence = new[]
            {
                _onDeath,
                _onAttack,
                _onCritAttack,
                _onMove,
                _onGetHit,
                _onGetCritHit
            };

            EffectSequenceHandler = new EffectSequenceHandler(this,effectSequence);
            
            StatusHandler.OnStatusEffectInterrupt += EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded += AddStatusEffectVisual;

            TargetingHandler.Init(this);
            
            if (_doShowHPBar)//Temp!
                Health.OnValueChanged += _hpBarConnector.SetBarToHealth;

            if (_doShowHPBar)
                _hpBarConnector.Init(this);
            else
                _hpBarConnector.gameObject.SetActive(false);
            
            gameObject.SetActive(true);

            _startedDeathSequence = false;

            IsTargetAble = true;
            
            IsInitialization = true;
        }
        
        public virtual void Init(UnitEntitySerializeData parameter, BaseUnitEntityVisualConfig visualConfig)
        {
            gameObject.name =  $"{parameter.EntityName} InstanceID: {EntityInstanceID}";
            
            Stats = new Dictionary<int, Stat>();

            foreach (var statConfig in parameter.StatSerializeDatas)
                Stats.Add(statConfig.ID ,new Stat(statConfig));
            
            DefaultPriorityTargeting =
                Factory.TargetingPriorityFactory.GetTargetingPriority(this, (TargetingPriorityType)parameter.TargetingPriority);
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);
            
            SpriteRenderer.sprite = visualConfig.Sprite;
            
            BaseInit();
        }
        
        [Obsolete("may need to use UnitEntitySerializeData only")]
        public virtual void Init(BaseUnitEntityConfig parameter)//need to oder logic to many responsibility
        {
            gameObject.name =  $"{parameter.name} InstanceID: {EntityInstanceID}";
            
            Stats = new Dictionary<int, Stat>();

            foreach (var statConfig in parameter.StatConfigs)
                Stats.Add(statConfig.ID,new Stat(statConfig));
            
            DefaultPriorityTargeting =
                Factory.TargetingPriorityFactory.GetTargetingPriority(this, parameter.TargetingPriority);
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);//making new every time we init new enemy(memory waste) 
            
            SpriteRenderer.sprite = parameter.UnitEntityVisualConfig.Sprite;
            
            BaseInit();
        }
        
        #endregion    
        
        #region UnityCallBacks
        
        protected override void Update()
        {
            base.Update();

            if (!IsInitialization)
                return;
            
            EffectSequenceHandler.UpdateEffectHandler();
            
            if (IsEntityDead)
            {
                if (_startedDeathSequence)
                    return;
                
                StartDeathSequence();
                return;
            }

            HealthComponentUpdate();
            StatusHandler.UpdateStatHandler();

            if (TargetingHandler.CurrentTarget == null || TargetingHandler.CurrentTarget.IsEntityDead)
                TargetingHandler.GetPriorityTarget();
            
            
            UpdateEntity();
        }

        protected abstract void UpdateEntity();

        private void OnValidate()
        {
            _soundHandler ??= GetComponentInChildren<SoundHandler>();
        }

        private void OnDrawGizmosSelected()
        {
            if (TargetingHandler != null)
            {
                if (TargetingHandler.CurrentTarget == null) return;
                
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position,TargetingHandler.CurrentTarget.EntityTransform.position);
                Gizmos.color = Color.white;
            }
        }

        protected virtual void OnDestroy()
        {
            if (!IsInitialization)
                return;

            StatusHandler.OnStatusEffectInterrupt -= EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded -= AddStatusEffectVisual;

            Health.OnValueChanged  -= _hpBarConnector.SetBarToHealth;
        }

        #endregion
        
        #region StatComponent

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders = new List<IStatHolder>() {this};
            
            foreach (var abilitiesValue in AbilityHandler.Abilities.Values)
                statHolders.AddRange(abilitiesValue.GetNestedStatHolders());

            return statHolders;
        }

        #endregion      
        
        #region TargetingComponent
        
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent)
            => Vector2.Distance(transform.position, targetAbleComponent.EntityTransform.position);

        #endregion

        #region HealthComponent
        
        public void Heal(float amount)
        {
            _healPopUpText_Config.damage = amount;
            _healPopUpText_Config.text = $"+{(int)amount}";
            _healPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(amount);
            
            _popUpTexter.SpawnPopUp(_healPopUpText_Config);
            Health.AddToValue(amount);
        }

        public void TakeDamage(float damage,bool isCrit)
        {
            if (IsDamageable)
            {
                EffectSequenceHandler.PlaySequenceById(isCrit
                    ? Constant.EffectSequenceIds.GET_CRIT_HIT
                    : Constant.EffectSequenceIds.GET_HIT);
                
                IsDamageable = false; // Is this what turns on InvincibleTime?
                if (isCrit)
                {
                    _critPopUpText_Config.damage = damage;
                    _critPopUpText_Config.text = $"{(int)damage}";
                    _critPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(damage);
                    _critPopUpText_Config.size += LevelVisualData_Monoton.Instance.Crit_FontSizeBonus; //this is pretty bad imo
                    _popUpTexter.SpawnPopUp(_critPopUpText_Config);
                }
                else
                {
                    _defaultPopUpText_Config.damage = damage;
                    _defaultPopUpText_Config.text = $"{(int)damage}";
                    _defaultPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(damage);
                    _popUpTexter.SpawnPopUp(_defaultPopUpText_Config);
                }
                
                Health.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce));
                IsDamageable = false;
            }
        }

        private void HealthComponentUpdate()
        {
            if (!IsDamageable)
            {
                _currentInvincibleTime -= GAME_TIME.GameDeltaTime;

                if (_currentInvincibleTime < 0)
                {
                    IsDamageable = true;
                    _currentInvincibleTime = InvincibleTime.CurrentValue;
                }
            }
        }

        #endregion

        #region CombatComponent                                                                                                                                   
        
        public void SetAttackTarget(IEntityTargetAbleComponent target) => TargetingHandler.SetAttackTarget(target);

        public abstract void Attack();

        public virtual void StartDeathSequence()
        {
            _startedDeathSequence = true;
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.ENTITY_COLOR}>{name}</color> as started death sequence");
#endif
            
            IsTargetAble = false;
            IsDamageable = false;
                
            OnTargetDisable?.Invoke(this);
            //EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.DEATH,EntityDied);
            EntityDied();
        }

        protected virtual void EntityDied()
        {
            IsInitialization = false;
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.ENTITY_COLOR}>{name}</color> as died!");
#endif
        }

        #endregion

        #region MovementComponent
        
        public void SetDestination(Vector3 destination, MoveType moveType) 
        {
            throw new System.NotImplementedException();
        }

        #endregion
        
        #region VisualComponent
        
        private void AddStatusEffectVisual(EffectSequenceConfig effectSequenceConfig) =>
            EffectSequenceHandler.PlaySequenceByData(effectSequenceConfig);//temp

        #endregion
    }
}