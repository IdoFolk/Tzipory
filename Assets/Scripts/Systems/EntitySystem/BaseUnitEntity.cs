using System.Collections.Generic;
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
    public abstract class BaseUnitEntity : BaseGameEntity , IEntityTargetAbleComponent , IEntityCombatComponent , IEntityMovementComponent , 
        IEntityTargetingComponent , IEntityAbilitiesComponent,IEntityVisualComponent,IInitialization<BaseUnitEntityConfig>
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

        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onDeath;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onCritAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onMove;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onGetHit;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceData _onGetCritHit;

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

            List<Stat> stats = new List<Stat>();
            
            stats.Add(new Stat(Constant.Stats.Health.ToString(), parameter.Health.BaseValue, parameter.Health.MaxValue,                         (int)Constant.Stats.Health));
            stats.Add(new Stat(Constant.Stats.InvincibleTime.ToString(), parameter.InvincibleTime.BaseValue, parameter.InvincibleTime.MaxValue, (int)Constant.Stats.InvincibleTime));
            stats.Add(new Stat(Constant.Stats.AttackDamage.ToString(), parameter.AttackDamage.BaseValue, parameter.AttackDamage.MaxValue,       (int)Constant.Stats.AttackDamage));
            stats.Add(new Stat(Constant.Stats.CritDamage.ToString(), parameter.CritDamage.BaseValue, parameter.CritDamage.MaxValue,             (int)Constant.Stats.CritDamage));
            stats.Add(new Stat(Constant.Stats.CritChance.ToString(), parameter.CritChance.BaseValue, parameter.CritChance.MaxValue,             (int)Constant.Stats.CritChance));
            stats.Add(new Stat(Constant.Stats.AttackRate.ToString(), parameter.AttackRate.BaseValue, parameter.AttackRate.MaxValue,             (int)Constant.Stats.AttackRate));
            stats.Add(new Stat(Constant.Stats.AttackRange.ToString(), parameter.AttackRange.BaseValue, parameter.AttackRange.MaxValue,          (int)Constant.Stats.AttackRange));
            stats.Add(new Stat(Constant.Stats.TargetingRange.ToString(), parameter.TargetingRange.BaseValue, parameter.TargetingRange.MaxValue, (int)Constant.Stats.TargetingRange));
            stats.Add(new Stat(Constant.Stats.MovementSpeed.ToString(), parameter.MovementSpeed.BaseValue, parameter.MovementSpeed.MaxValue,    (int)Constant.Stats.MovementSpeed));
            
            if (parameter.Stats != null && parameter.Stats.Count > 0)
            {
                foreach (var stat in parameter.Stats)
                    stats.Add(new Stat(stat.Name, stat.BaseValue, stat.MaxValue, stat.Id));
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
            
            var effectSequence = new EffectSequenceData[]
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
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