using System;
using System.Collections.Generic;
using GameplayLogic.UI.HPBar;
using MyNamespaceTzipory.Systems.VisualSystem;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using Sirenix.OdinInspector;
using Spine.Unity;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.EntitySystem.EntityVisual;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Tools.TimeSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.AnimationSystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;

namespace Tzipory.Systems.Entity
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
        [SerializeField,TabGroup("Component")] private AnimationHandler _animationHandler;
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

        #region Visual Events
        public Action<bool> OnSpriteFlipX;
        public Action<Sprite> OnSetSprite;

        #endregion

        private float  _currentInvincibleTime;

        private bool _startedDeathSequence;

        #endregion

        #region Proprty

        public Dictionary<int, Stat> Stats { get; private set; }
        
        public StatusHandler StatusHandler { get; private set; }
        
        public AbilityHandler AbilityHandler { get; private set; }
        
        public EffectSequenceHandler EffectSequenceHandler { get; private set; }
        public AnimationHandler AnimationHandler => _animationHandler;
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
            
            StatusHandler.OnStatusEffectAdded += AddStatusEffectVisual;

            TargetingHandler.Init(this);
            
            if (_doShowHPBar)//Temp!
                Health.OnValueChangedData += _hpBarConnector.SetBarToHealth;

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

            foreach (var statSerializeData in parameter.StatSerializeDatas)
                Stats.Add(statSerializeData.ID ,new Stat(statSerializeData));
            
            DefaultPriorityTargeting =
                Systems.FactorySystem.ObjectFactory.TargetingPriorityFactory.GetTargetingPriority(this, (TargetingPriorityType)parameter.TargetingPriority);
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);

            //temp, spine currently in implementation
            if (visualConfig.SkeletonDataAsset is null)
            {
                _spriteRenderer.enabled = true;
                _animationHandler.gameObject.SetActive(false);
                SetSprite(visualConfig.Sprite);
            }
            else
            {
                _spriteRenderer.enabled = false;
                _animationHandler.gameObject.SetActive(true);
                _animationHandler.Init(visualConfig.SkeletonDataAsset);
            }
            //
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
                Systems.FactorySystem.ObjectFactory.TargetingPriorityFactory.GetTargetingPriority(this, parameter.TargetingPriority);
            
            AbilityHandler = new AbilityHandler(this,this, parameter.AbilityConfigs);//making new every time we init new enemy(memory waste) 
            
            //temp, spine currently in implementation
            if (parameter.UnitEntityVisualConfig.SkeletonDataAsset is null)
            {
                _spriteRenderer.enabled = true;
                _animationHandler.gameObject.SetActive(false);
                SetSprite(parameter.UnitEntityVisualConfig.Sprite);
            }
            else
            {
                _spriteRenderer.enabled = false;
                _animationHandler.gameObject.SetActive(true);
                _animationHandler.Init(parameter.UnitEntityVisualConfig.SkeletonDataAsset);
            }
            //
            
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
            
            StatusHandler.UpdateStatHandler();
            HealthComponentUpdate();
            
            if (IsEntityDead)
            {
                if (_startedDeathSequence)
                    return;
                
                StartDeathSequence();
                return;
            }
            
            if (TargetingHandler.CurrentTarget == null || TargetingHandler.CurrentTarget.IsEntityDead)
                TargetingHandler.GetPriorityTarget();
            
            UpdateEntity();
        }

        protected abstract void UpdateEntity();

        private void OnValidate()
        {
            _soundHandler ??= GetComponentInChildren<SoundHandler>();
            _animationHandler ??= GetComponentInChildren<AnimationHandler>();
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

            StatusHandler.OnStatusEffectAdded -= AddStatusEffectVisual;

            Health.OnValueChangedData  -= _hpBarConnector.SetBarToHealth;
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
            Health.ProcessStatModifier(new StatModifier(amount,StatusModifierType.Addition));
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
            TargetingHandler.Reset();
            EffectSequenceHandler.Reset();
#if UNITY_EDITOR
            Debug.Log($"<color={ColorLogHelper.ENTITY_COLOR}>{name}</color> as died!");
#endif
        }

        #endregion

        #region MovementComponent
        
        public void SetDestination(Vector3 destination, MoveType moveType) 
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region VisualComponent
        
        private void AddStatusEffectVisual(EffectSequenceConfig effectSequenceConfig) =>
            EffectSequenceHandler.PlaySequenceByData(effectSequenceConfig);//temp

        private void SetSprite(Sprite newSprite)
        {
            _animationHandler.gameObject.SetActive(false);
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = newSprite;
            OnSetSprite?.Invoke(newSprite);
        }
        public void SetSpriteFlipX(bool doFlip)
        {
            if (_animationHandler.gameObject.activeSelf)
            {
                _animationHandler.FlipSkeletonAnimation(doFlip);
                return;
            }
            
            _spriteRenderer.flipX = doFlip;
            OnSpriteFlipX?.Invoke(doFlip);
        }

        

        #endregion
    }
}