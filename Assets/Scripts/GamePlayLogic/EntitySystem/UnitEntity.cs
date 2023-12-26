using System;
using System.Collections.Generic;
using GameplayLogic.UI.HPBar;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent;
using Tzipory.Helpers;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.FactorySystem;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using UnityEngine;

namespace Tzipory.GamePlayLogic.EntitySystem
{
    public class UnitEntity : BaseGameEntity, ITargetAbleEntity, IInitialization<UnitEntityConfig>, IInitialization<UnitEntitySerializeData>, IPoolable<UnitEntity>
    {
        public event Action<UnitEntity> OnDispose;
        public event Action<ITargetAbleEntity> OnTargetDisable;

        #region Fields

#if UNITY_EDITOR
        [SerializeField, ReadOnly, TabGroup("StatsId")]
        private List<Stat> _stats;
#endif
        [SerializeField, TabGroup("Component")]
        private SoundHandler _soundHandler;

        [SerializeField, TabGroup("Component")]
        private TargetingComponent _entityTargetingComponent; //temp

        [SerializeField, TabGroup("Component")]
        private ColliderTargetingArea _colliderTargeting;

        [SerializeField, TabGroup("Component")]
        private BoxCollider2D _boxCollider;

        [SerializeField, TabGroup("Component")]
        private CircleCollider2D _groundCollider;

        [SerializeField, TabGroup("Component")]
        private AgentMoveComponent _agentMoveComponent;

        [SerializeField, TabGroup("Component")]
        private Animator _entityAnimator;
        
        [SerializeField, TabGroup("Component")]
        private EntityAnimatorEventReader eventReader;
        
        [SerializeField,TabGroup("Component")] private ClickHelper _clickHelper;

        [Header("Visual components")] [SerializeField, TabGroup("Component")]
        private UnitEntityVisualComponent _entityVisualComponent; //temp

        #region Temps

        [Header("Temps"), TabGroup("Component")] [SerializeField, TabGroup("Component")]
        private TEMP_UNIT_HPBarConnector _hpBarConnector;

        #endregion

        private UnitEntitySerializeData _serializeData;
        private UnitEntityConfig _config;

        private bool _isUsingConfig;

        #endregion

        #region Proprty

        public Animator EntityAnimator => _entityAnimator;
        public CircleCollider2D GroundCollider => _groundCollider;
        public IEntityVisualComponent EntityVisualComponent => _entityVisualComponent;
        public IEntityTargetingComponent EntityTargetingComponent => _entityTargetingComponent;
        public IEntityMovementComponent EntityMovementComponent { get; private set; }
        public IEntityAbilitiesComponent EntityAbilitiesComponent { get; private set; }
        public IEntityHealthComponent EntityHealthComponent { get; private set; }
        public IEntityStatComponent EntityStatComponent { get; private set; }
        public IEntityCombatComponent EntityCombatComponent { get; private set; }
        public IEntityAnimatorComponent EntityAnimatorComponent { get; private set; }

        public IEntityExperienceComponent EntityExperienceComponent { get; private set; }
        public Action<UnitEntity> OnKill;
        private IEntityAIComponent EntityAIComponent { get; set; }

        public bool IsTargetAble { get; set; }
        public EntityType EntityType { get; private set; }

        public bool IsInitialization { get; private set; }

        public UnitEntityConfig Config => _config;

        public void Init(BaseGameEntity parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Inits

        public void Init(UnitEntitySerializeData parameter)
        {
            _serializeData = parameter;

            if (!_isUsingConfig)
                _config = DataManager.DataRequester.GetConfigData<UnitEntityConfig>(parameter);
            
            gameObject.name = $"{_config.name} InstanceID: {EntityInstanceID}";
            
            _clickHelper.gameObject.SetActive(_config.UnitType != UnitType.Enemy);

            gameObject.tag = _config.UnitType.ToString();
            _groundCollider.gameObject.tag = _config.UnitType.ToString();
            _boxCollider.enabled = true;

            List<IStatHolder> statHolders = new List<IStatHolder>();

            AddComponent(EntityVisualComponent);
            AddComponent(EntityTargetingComponent);
            _entityTargetingComponent.gameObject.SetActive(true);

            statHolders.Add(EntityTargetingComponent);

            EntityHealthComponent = HealthComponentFactory.GetHealthComponent(_config.HealthComponentConfig);
            AddComponent(EntityHealthComponent);
            statHolders.Add(EntityHealthComponent);
            EntityHealthComponent.OnDeath += Dispose;

            if (_config.CombatComponent)
            {
                EntityCombatComponent = CombatComponentFactory.GetCombatComponent(_config.CombatComponentConfig);
                AddComponent(EntityCombatComponent);
                statHolders.Add(EntityCombatComponent);
            }

            if (_config.AbilityComponent)
            {
                EntityAbilitiesComponent = AbilityComponentFactory.GetAbilitiesComponent(_config.AbilityComponentConfig);
                AddComponent(EntityAbilitiesComponent);
                statHolders.Add(EntityAbilitiesComponent);
            }

            if (_config.MovementComponent)
            {
                EntityMovementComponent = MovementComponentFactory.GetMovementComponent(_config.MovementComponentConfig);
                AddComponent(EntityMovementComponent);
                statHolders.Add(EntityMovementComponent);
            }

            if (_config.AIComponent)
            {
                EntityAIComponent = AIComponentFactory.GetAIComponent(_config.AIComponentConfig);
                AddComponent(EntityAIComponent);
            }

            if (_config.AnimatorComponent)
            {
                EntityAnimatorComponent = AnimatorComponentFactory.GetAIComponent(_config.AnimatorComponentConfig);
                AddComponent(EntityAnimatorComponent);
                eventReader.OnDeathAnimationEnded += DisableGameObject;
            }

            EntityStatComponent = new StatHandlerComponent(); //may need to work in init!
            AddComponent(EntityStatComponent);

            EntityStatComponent.OnStatusEffectAdded += AddStatusEffectVisual;

            EntityVisualComponent.Init(this, _config.VisualComponentConfig);
            EntityHealthComponent.Init(this, _config.HealthComponentConfig);
            EntityTargetingComponent.Init(this, _colliderTargeting, _config.TargetingComponentConfig);
            EntityAbilitiesComponent?.Init(this, _config.AbilityComponentConfig);
            EntityMovementComponent?.Init(this, _config.MovementComponentConfig, _agentMoveComponent);
            EntityCombatComponent?.Init(this, _config.CombatComponentConfig);
            EntityAnimatorComponent?.Init(this, this, _config.AnimatorComponentConfig);
            EntityAIComponent?.Init(this, this, _config.AIComponentConfig);

            EntityType = _config.TargetingComponentConfig.EntityType;

            if (EntityType == EntityType.None)
                throw new Exception($"{GameEntity.name} as None in is Entitytype");

            EntityStatComponent.Init(this, statHolders);

            foreach (var stat in EntityStatComponent.GetAllStats())
            {
                stat.OnValueChanged += EntityVisualComponent.PopUpTexter.SpawnPopUp;
#if UNITY_EDITOR
                _stats.Add(stat);
#endif
            }

            #region Temp

            if (_config.VisualComponentConfig.HpBar)
            {
                _hpBarConnector.gameObject.SetActive(true);
                _hpBarConnector.Init(this,EntityType);
                EntityHealthComponent.Health.OnValueChanged += _hpBarConnector.SetBarToHealth;
            }
            else
                _hpBarConnector.gameObject.SetActive(false);

            #endregion

            IsTargetAble = true;

            gameObject.SetActive(true);

            IsInitialization = true;
        }

        [Obsolete("may need to use UnitEntitySerializeData only")]
        public void Init(UnitEntityConfig parameter) //need to oder logic to many responsibility
        {
            var serializeData = DataManager.DataRequester.GetSerializeData<UnitEntitySerializeData>(parameter);

            _isUsingConfig = true;

            _config = parameter;
            
            Init(serializeData);
        }

        #endregion

        #region UnityCallBacks

        private void OnValidate()
        {
            _soundHandler ??= GetComponentInChildren<SoundHandler>();
            _entityTargetingComponent ??= GetComponentInChildren<TargetingComponent>();
            _colliderTargeting ??= GetComponentInChildren<ColliderTargetingArea>();
            _entityVisualComponent ??= GetComponentInChildren<UnitEntityVisualComponent>();
            _hpBarConnector ??= GetComponentInChildren<TEMP_UNIT_HPBarConnector>();
            _agentMoveComponent ??= GetComponent<AgentMoveComponent>();
            _entityAnimator ??= GetComponentInChildren<Animator>();
            _boxCollider ??= GetComponent<BoxCollider2D>();
        }

        private void OnDrawGizmosSelected()
        {
            if (EntityTargetingComponent != null)
            {
                if (EntityTargetingComponent.CurrentTarget == null) return;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, EntityTargetingComponent.CurrentTarget.GameEntity.transform.position);
                Gizmos.color = Color.white;
            }
        }

        private void OnDestroy()
        {
            if (!IsInitialization)
                return;

            EntityStatComponent.OnStatusEffectAdded -= AddStatusEffectVisual;

            EntityHealthComponent.Health.OnValueChanged -= _hpBarConnector.SetBarToHealth;
            EntityHealthComponent.OnDeath -= Dispose;
            
            if (_config.AnimatorComponent) eventReader.OnDeathAnimationEnded -= DisableGameObject;

            foreach (var stat in EntityStatComponent.GetAllStats())
                stat.OnValueChanged -= EntityVisualComponent.PopUpTexter.SpawnPopUp;
        }

        #endregion

        #region VisualComponent

        private void AddStatusEffectVisual(EffectSequenceConfig effectSequenceConfig) =>
            EntityVisualComponent.EffectSequenceHandler.PlaySequenceByData(effectSequenceConfig); //temp

        #endregion

        public void Free()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (!_config.AnimatorComponent) gameObject.SetActive(false);
            _agentMoveComponent.Agent.enabled = false;
            _entityTargetingComponent.gameObject.SetActive(false);
            _hpBarConnector.gameObject.SetActive(false);
            _boxCollider.enabled = false;
            IsInitialization = false;
            IsTargetAble = false;
            OnDispose?.Invoke(this);
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}