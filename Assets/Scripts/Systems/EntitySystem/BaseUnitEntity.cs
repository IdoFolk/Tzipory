﻿using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using Tzipory.VisualSystem.EffectSequence;
using UnityEngine;

namespace Tzipory.EntitySystem.Entitys
{
    public abstract class BaseUnitEntity : BaseGameEntity, IEntityTargetAbleComponent, IEntityCombatComponent, IEntityMovementComponent, 
        IEntityTargetingComponent, IEntityAbilitiesComponent, IEntityVisualComponent, IInitialization<BaseUnitEntityConfig>
    {
        #region Fields

#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("Stats")] private List<Stat> _stats;
#endif
        [SerializeField,TabGroup("Component")] private CircleCollider2D _bodyCollider;
        [Header("Visual components")]
        [SerializeField,TabGroup("Component")] private SpriteRenderer _spriteRenderer;
        [SerializeField,TabGroup("Component")] private Transform _visualQueueEffectPosition;
        [SerializeField,TabGroup("Component")] private Transform _particleEffectPosition;
        [SerializeField,TabGroup("Component")] private SoundHandler _soundHandler;

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

        #endregion

        #region Temps
        [Header("TEMPS")]
        [SerializeField] private bool _doShowHPBar;
        [SerializeField] private TEMP_UNIT_HPBarConnector _hpBarConnector;
        #endregion

        //Temp?
        #region Events
        //public event Action OnHealthChanged;
        #endregion

        #region UnityCallBacks
        
        public bool IsInitialization { get; private set; }

        public virtual void Init(BaseUnitEntityConfig parameter)//need to oder logic to many responsibility
        {
            gameObject.name =  $"{parameter.name} InstanceID: {EntityInstanceID}";

            List<Stat> stats = new List<Stat>
            {
                new Stat(parameter.Health),
                new Stat(parameter.InvincibleTime),
                new Stat(parameter.AttackDamage),
                new Stat(parameter.CritDamage),
                new Stat(parameter.CritChance),
                new Stat(parameter.AttackRate),
                new Stat(parameter.AttackRange),
                new Stat(parameter.TargetingRange),
                new Stat(parameter.MovementSpeed)
            };

            if (parameter.Stats is { Count: > 0 })
            {
                foreach (var stat in parameter.Stats)
                    stats.Add(stat);
            }
            
#if UNITY_EDITOR
            _stats = stats;
#endif
               
            StatusHandler = new StatusHandler(stats,this);//may need to work in init!
            
            DefaultPriorityTargeting =
                Factory.TargetingPriorityFactory.GetTargetingPriority(this, parameter.TargetingPriority);
            
            Targeting.Init(this);
            
            StatusHandler.OnStatusEffectInterrupt += EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded += AddStatusEffectVisual;
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);
            
            SpriteRenderer.sprite = parameter.Sprite;
            
            //init Hp_bar
            if (_doShowHPBar)//Temp!
                HP.OnValueChanged += _hpBarConnector.SetBarToHealth;

            if (_doShowHPBar)
                _hpBarConnector.Init(this);
            else
                _hpBarConnector.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
            IsInitialization = true;
        }

        protected override void Awake()
        {
            base.Awake();
            Targeting = GetComponentInChildren<TargetingHandler>();//temp
            
            _onDeath.ID = Constant.EffectSequenceIds.OnDeath;
            _onAttack.ID = Constant.EffectSequenceIds.OnAttack;
            _onCritAttack.ID = Constant.EffectSequenceIds.OnCritAttack;
            _onMove.ID = Constant.EffectSequenceIds.OnMove;
            _onGetHit.ID = Constant.EffectSequenceIds.OnGetHit;
            _onGetCritHit.ID = Constant.EffectSequenceIds.OnGetCritHit;
            
            _onDeath.SequenceName = "OnDeath";
            _onAttack.SequenceName = "OnAttack";
            _onCritAttack.SequenceName = "OnCritAttack";
            _onMove.SequenceName = "OnMove";
            _onGetHit.SequenceName = "OnGetHit";
            _onGetCritHit.SequenceName = "OnGetCritHit";
            
            var effectSequence = new EffectSequenceConfig[]
            {
                _onDeath,
                _onAttack,
                _onCritAttack,
                _onMove,
                _onGetHit,
                _onGetCritHit
            };

            EffectSequenceHandler = new EffectSequenceHandler(this,effectSequence);
        }


        protected override void Update()
        {
            base.Update();

            if (!IsInitialization)
                return;

            HealthComponentUpdate();
            StatusHandler.UpdateStatusEffects();

            if (Targeting.CurrentTarget == null || Targeting.CurrentTarget.IsEntityDead)
                Targeting.GetPriorityTarget();
            
            EffectSequenceHandler.UpdateEffectHandler();
            
            UpdateEntity();
        }

        protected abstract void UpdateEntity();

        private void OnValidate()
        {
            _soundHandler ??= GetComponentInChildren<SoundHandler>();

            // if (_bodyCollider == null || _rangeCollider == null)
            // {
            //     var colliders = GetComponents<CircleCollider2D>();
            //
            //     foreach (var collider in colliders)
            //     {
            //         if (collider.isTrigger)
            //             _rangeCollider = collider;
            //         else
            //             _bodyCollider = collider;
            //     }
            // }
        }

        private void OnDrawGizmosSelected()
        {
           // Gizmos.DrawWireSphere(transform.position,_config.AttackRange.BaseValue / 2);

            if (Targeting != null)
            {
                if (Targeting.CurrentTarget == null) return;
                
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position,Targeting.CurrentTarget.EntityTransform.position);
                Gizmos.color = Color.white;
            }
        }

        protected virtual void OnDestroy()
        {
            if (!IsInitialization)
                return;

            StatusHandler.OnStatusEffectInterrupt -= EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded -= AddStatusEffectVisual;

            HP.OnValueChanged  -= _hpBarConnector.SetBarToHealth;
        }

        #endregion

        #region TargetingComponent

        public Stat TargetingRange => StatusHandler.GetStatById((int)Constant.Stats.TargetingRange);
        public bool IsTargetAble { get; }
        public EntityTeamType EntityTeamType { get; protected set; }
        public IPriorityTargeting DefaultPriorityTargeting { get; private set; }
        public TargetingHandler Targeting { get; set; }
        
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent)
            => Vector2.Distance(transform.position, targetAbleComponent.EntityTransform.position);

        #endregion

        #region HealthComponent
        
        private float  _currentInvincibleTime;

        public Stat HP => StatusHandler.GetStatById((int)Constant.Stats.Health);
        public Stat InvincibleTime => StatusHandler.GetStatById((int)Constant.Stats.InvincibleTime);
        public bool IsDamageable { get; private set; }
        public bool IsEntityDead => HP.CurrentValue <= 0;

        public void Heal(float amount)
        {
            HP.AddToValue(amount);
            _popUpTexter.SpawnPopUp($"+{amount}", _healPopUpText_Config);
            //OnHealthChanged?.Invoke();
            // if (HP.CurrentValue > HP.BaseValue)
            //     HP.ResetValue();
        }

        public void TakeDamage(float damage,bool isCrit)
        {
            if (IsDamageable)
            {
                EffectSequenceHandler.PlaySequenceById(isCrit
                    ? Constant.EffectSequenceIds.OnGetCritHit
                    : Constant.EffectSequenceIds.OnGetHit);
                
                HP.ReduceFromValue(damage);
                IsDamageable = false; // Is this what turns on InvincibleTime?
                if (isCrit)
                {
                    _popUpTexter.SpawnPopUp($"-{damage}", _defaultPopUpText_Config);
                }
                else
                {
                    _popUpTexter.SpawnPopUp($"-{damage}!", _critPopUpText_Config);
                }
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

            if (HP.CurrentValue < 0)
                OnEntityDead();
        }

        #endregion

        #region CombatComponent                                                                                                                                   
        
        public void SetAttackTarget(IEntityTargetAbleComponent target) => Targeting.SetAttackTarget(target);

        public Stat AttackDamage => StatusHandler.GetStatById((int)Constant.Stats.AttackDamage);
        public Stat CritDamage => StatusHandler.GetStatById((int)Constant.Stats.CritDamage);
        public Stat CritChance => StatusHandler.GetStatById((int)Constant.Stats.CritChance);
        public Stat AttackRate => StatusHandler.GetStatById((int)Constant.Stats.AttackRate);
        public Stat AttackRange => StatusHandler.GetStatById((int)Constant.Stats.AttackRange);

        public abstract void Attack();
        public abstract void OnEntityDead();

        #endregion

        #region MovementComponent


        public Stat MoveSpeed => StatusHandler.GetStatById((int)Constant.Stats.MovementSpeed);
        
        //This is not really needed - we can remove the movement interface from baseunit I think... - it should have a BasicMovement, controlled by something else
        public void SetDestination(Vector3 destination, MoveType moveType) 
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region StstusEffcetComponent

        //stuff
        public StatusHandler StatusHandler { get; private set; }

        #endregion

        #region AbilityComponent
        
        public AbilityHandler AbilityHandler { get; private set; }

        #endregion
        
        #region VisualComponent
        
        public EffectSequenceHandler EffectSequenceHandler { get; private set; }
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public SoundHandler SoundHandler => _soundHandler;
        public Transform ParticleEffectPosition => _particleEffectPosition;
        public Transform VisualQueueEffectPosition => _visualQueueEffectPosition;

        private void AddStatusEffectVisual(BaseStatusEffect baseStatusEffect) =>
            EffectSequenceHandler.PlaySequenceByData(baseStatusEffect.EffectSequence);//temp

        #endregion
    }
}