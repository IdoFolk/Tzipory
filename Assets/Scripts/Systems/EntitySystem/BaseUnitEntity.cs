using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using Tzipory.VisualSystem.EffectSequence;
using UnityEngine;

namespace Tzipory.EntitySystem.Entitys
{
    public abstract class BaseUnitEntity : BaseGameEntity, IEntityTargetAbleComponent, IEntityCombatComponent, IEntityMovementComponent, 
        IEntityTargetingComponent, IEntityAbilitiesComponent, IEntityVisualComponent, IInitialization<BaseUnitEntityConfig> , IInitialization<UnitEntitySerializeData,BaseUnitEntityVisualConfig>
    {
        #region Fields

#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("Stats")] private List<Stat> _stats;
#endif
        [SerializeField,TabGroup("Component")] private Transform _particleEffectPosition;
        [SerializeField,TabGroup("Component")] private SoundHandler _soundHandler;
        [SerializeField,TabGroup("Component")] private TargetingHandler _targetingHandler;
        [Header("Visual components")]
        [SerializeField,TabGroup("Component")] private SpriteRenderer _spriteRenderer;
        [SerializeField,TabGroup("Component")] private Transform _visualQueueEffectPosition;
        [SerializeField, TabGroup("Component")] private SpriteRenderer _silhouetteRenderer;


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
        [Header("TEMPS")] [SerializeField]
        private Transform _shotPosition;
        [SerializeField] private bool _doShowHPBar;
        [SerializeField] private TEMP_UNIT_HPBarConnector _hpBarConnector;

        //The current front-most (highest z value) obstacle
        float CurrentTopObstacleZ()
        {
            float toReturn = float.PositiveInfinity;
            if (_obstaclesZs == null || _obstaclesZs.Count == 0)
                return toReturn;

            foreach (var item in _obstaclesZs)
            {
                if(item < toReturn)
                    toReturn = item;
            }
            return toReturn;
        }
        List<float> _obstaclesZs = new List<float>();

        public void AddObstacleZ(float z)
        {
            _obstaclesZs.Add(z);
            SilhouetteSpriteRenderer.material.SetFloat("_ObstacleZ", CurrentTopObstacleZ());
        }
        public void RemoveObstacleZ(float z)
        {
            _obstaclesZs.Remove(z);
            SilhouetteSpriteRenderer.material.SetFloat("_ObstacleZ", CurrentTopObstacleZ());
        }

        #endregion

        //Temp?
        #region Events
        //public event Action OnHealthChanged;
        #endregion

        #region UnityCallBacks

        public bool IsInitialization { get; private set; }
        
        public virtual void Init(UnitEntitySerializeData parameter, BaseUnitEntityVisualConfig visualConfig)
        {
            gameObject.name =  $"{parameter.EntityName} InstanceID: {EntityInstanceID}";

            List<Stat> stats = new List<Stat>
            {
                new(parameter.Health),
                new(parameter.InvincibleTime),
                new(parameter.AttackDamage),
                new(parameter.CritDamage),
                new(parameter.CritChance),
                new(parameter.AttackRate),
                new(parameter.AttackRange),
                new(parameter.TargetingRange),
                new(parameter.MovementSpeed)
            };

            // if (parameter.Stats is { Count: > 0 })
            // {
            //     foreach (var stat in parameter.Stats)
            //         stats.Add(stat);
            // }
            
#if UNITY_EDITOR
            _stats = stats;
#endif
               
            StatusHandler = new StatusHandler(stats,this);//may need to work in init!
            
            DefaultPriorityTargeting =
                Factory.TargetingPriorityFactory.GetTargetingPriority(this, (TargetingPriorityType)parameter.TargetingPriority);
            
            TargetingHandler.Init(this);
            
            StatusHandler.OnStatusEffectInterrupt += EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded += AddStatusEffectVisual;
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);

            SetUnitSprite(visualConfig.Sprite);

            //init Hp_bar
            if (_doShowHPBar)//Temp!
                Health.OnValueChanged += _hpBarConnector.SetBarToHealth;

            if (_doShowHPBar)
                _hpBarConnector.Init(this);
            else
                _hpBarConnector.gameObject.SetActive(false);
            
            gameObject.SetActive(true);
            IsInitialization = true;
        }

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
            
            TargetingHandler.Init(this);
            
            StatusHandler.OnStatusEffectInterrupt += EffectSequenceHandler.RemoveEffectSequence;
            StatusHandler.OnStatusEffectAdded += AddStatusEffectVisual;
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);

            SetUnitSprite(parameter.UnitEntityVisualConfig.Sprite);
            //init Hp_bar
            if (_doShowHPBar)//Temp!
                Health.OnValueChanged += _hpBarConnector.SetBarToHealth;

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

            if (TargetingHandler.CurrentTarget == null || TargetingHandler.CurrentTarget.IsEntityDead)
                TargetingHandler.GetPriorityTarget();
            
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

        #region TargetingComponent

        public Stat TargetingRange => StatusHandler.GetStatById((int)Constant.Stats.TargetingRange);
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; protected set; }
        public Vector2 ShotPosition => _shotPosition.position;
        public IPriorityTargeting DefaultPriorityTargeting { get; private set; }
        public TargetingHandler TargetingHandler => _targetingHandler;
        
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent)
            => Vector2.Distance(transform.position, targetAbleComponent.EntityTransform.position);

        #endregion

        #region HealthComponent
        
        private float  _currentInvincibleTime;

        public Stat Health => StatusHandler.GetStatById((int)Constant.Stats.Health);
        public Stat InvincibleTime => StatusHandler.GetStatById((int)Constant.Stats.InvincibleTime);
        public bool IsDamageable { get; private set; }
        public bool IsEntityDead => Health.CurrentValue <= 0;

        public void Heal(float amount)
        {
            _healPopUpText_Config.damage = amount;
            _healPopUpText_Config.text = $"+{amount}";
            _healPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(amount);
            
            _popUpTexter.SpawnPopUp(_healPopUpText_Config);
            //_popUpTexter.SpawnPopUp($"+{amount}", _healPopUpText_Config);
            Health.AddToValue(amount);
            //OnHealthChanged?.Invoke();
            // if (Health.CurrentValue > Health.BaseValue)
            //     Health.ResetValue();
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
                    _critPopUpText_Config.text = $"{damage}";
                    _critPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(damage);
                    _critPopUpText_Config.size += LevelVisualData_Monoton.Instance.Crit_FontSizeBonus; //this is pretty bad imo
                    _popUpTexter.SpawnPopUp(_critPopUpText_Config);
                }
                else
                {
                    //_defaultPopUpText_Config.text = $"-{damage}";
                    _defaultPopUpText_Config.damage = damage;
                    _defaultPopUpText_Config.text = $"{damage}";
                    _defaultPopUpText_Config.size = LevelVisualData_Monoton.Instance.GetRelativeFontSizeForDamage(damage);
                    _popUpTexter.SpawnPopUp(_defaultPopUpText_Config);
                }
                Health.ReduceFromValue(damage);
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

            if (Health.CurrentValue < 0)
                OnEntityDead();
        }

        #endregion

        #region CombatComponent                                                                                                                                   
        
        public void SetAttackTarget(IEntityTargetAbleComponent target) => TargetingHandler.SetAttackTarget(target);

        public Stat AttackDamage => StatusHandler.GetStatById((int)Constant.Stats.AttackDamage);
        public Stat CritDamage => StatusHandler.GetStatById((int)Constant.Stats.CritDamage);
        public Stat CritChance => StatusHandler.GetStatById((int)Constant.Stats.CritChance);
        public Stat AttackRate => StatusHandler.GetStatById((int)Constant.Stats.AttackRate);
        public Stat AttackRange => StatusHandler.GetStatById((int)Constant.Stats.AttackRange);

        public abstract void Attack();
        public abstract void OnEntityDead();

        #endregion

        #region MovementComponent


        public Stat MovementSpeed => StatusHandler.GetStatById((int)Constant.Stats.MovementSpeed);
        
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
        public SpriteRenderer SilhouetteSpriteRenderer => _silhouetteRenderer;
        public SoundHandler SoundHandler => _soundHandler;
        public Transform ParticleEffectPosition => _particleEffectPosition;
        public Transform VisualQueueEffectPosition => _visualQueueEffectPosition;
        public PopUpTexter PopUpTexter => _popUpTexter;

        //The following bellow is awaiting approval

        /// <summary>
        /// Unifies the main sprite setting method.
        /// Helps in setting additional sprites to that same sprtie.
        /// Can also be turned into an event Action<Sprite> and any new sprite renderers (like the shadow) could sub to the SetMainSprtie<Sprite> Action
        /// but I don't forsee any more renderers of this type - so I don't think it is needed at this time
        /// </summary>
        /// <param name="newSprite"></param>
        public void SetUnitSprite(Sprite newSprite)
        {
            SpriteRenderer.sprite = newSprite;
            _silhouetteRenderer.sprite = newSprite; 
        }
        /// <summary>
        /// Flips all sprites that maybe
        /// </summary>
        /// <param name="newSprite"></param>
        public void SetSpriteFlipX(bool doFlip)
        {
            SpriteRenderer.flipX = doFlip;
            _silhouetteRenderer.flipX = doFlip; 
        }

        private void AddStatusEffectVisual(BaseStatusEffect baseStatusEffect) =>
            EffectSequenceHandler.PlaySequenceByData(baseStatusEffect.EffectSequence);//temp

        #endregion
    }
}