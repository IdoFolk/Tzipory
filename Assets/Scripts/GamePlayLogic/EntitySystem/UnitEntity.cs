using System;
using System.Collections.Generic;
using GameplayLogic.UI.HPBar;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent;
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
    public class UnitEntity : BaseGameEntity, ITargetAbleEntity, IInitialization<UnitEntityConfig> , IInitialization<UnitEntitySerializeData>, IPoolable<UnitEntity>
    {
        public event Action<UnitEntity> OnDispose;
        public event Action<ITargetAbleEntity> OnTargetDisable;
        
        #region Visual Events
        
        public Action<bool> OnSpriteFlipX;
        public Action<Sprite> OnSetSprite;

        #endregion
        
        #region Fields
        
#if UNITY_EDITOR
        [SerializeField, ReadOnly,TabGroup("StatsId")] private List<Stat> _stats;
#endif
        [SerializeField,TabGroup("Component")] private SoundHandler _soundHandler;
        [SerializeField,TabGroup("Component")] private TargetingComponent _entityTargetingComponent;//temp
        [SerializeField,TabGroup("Component")] private ColliderTargetingArea _colliderTargeting;
        [SerializeField,TabGroup("Component")] private AgentMoveComponent _agentMoveComponent;
        [Header("Visual components")]
        [SerializeField,TabGroup("Component")] private UnitEntityVisualComponent _entityVisualComponent;//temp
        
        #region Temps
        
        [Header("Temps"),TabGroup("Component")] 
        [SerializeField,TabGroup("Component")] private TEMP_UNIT_HPBarConnector _hpBarConnector;
        
        #endregion

        private List<IEntityComponent> _entityComponent;
        
        private UnitEntitySerializeData _serializeData;
        private UnitEntityConfig _config;

        #endregion

        #region Proprty
        
        public IEntityVisualComponent EntityVisualComponent => _entityVisualComponent;
        public IEntityTargetingComponent EntityTargetingComponent => _entityTargetingComponent;
        public IEntityMovementComponent EntityMovementComponent { get; private set; }
        public IEntityAbilitiesComponent EntityAbilitiesComponent { get; private set; }
        public IEntityHealthComponent EntityHealthComponent { get; private set; }
        public IEntityStatComponent EntityStatComponent { get; private set; }
        public IEntityCombatComponent  EntityCombatComponent { get; private set; }
        public IEntityExperienceComponent EntityExperienceComponent { get; private set; }
        private IEntityAIComponent EntityAIComponent { get; set; }
        
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; }
        
        public bool IsInitialization { get; private set; }
        
        public void Init(BaseGameEntity parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region Inits
        
        public virtual void Init(UnitEntitySerializeData parameter)
        {
            _serializeData = parameter;
            _config = DataManager.DataRequester.GetConfigData<UnitEntityConfig>(parameter);
            
            gameObject.name =  $"{_config.name} InstanceID: {EntityInstanceID}";

            _entityComponent = new List<IEntityComponent>();
            List<IStatHolder> statHolders = new List<IStatHolder>();
            
            
            EntityVisualComponent.Init(this,_config.VisualComponentConfig);
            _entityComponent.Add(EntityVisualComponent);
            
            EntityTargetingComponent.Init(this,_colliderTargeting,_config.TargetingComponentConfig);
            _entityComponent.Add(EntityTargetingComponent);
            statHolders.Add(EntityTargetingComponent);
            
            EntityHealthComponent = HealthComponentFactory.GetHealthComponent(_config.HealthComponentConfig);
            EntityHealthComponent.Init(this,_config.HealthComponentConfig);
            _entityComponent.Add(EntityHealthComponent);
            statHolders.Add(EntityHealthComponent);
            
            if (_config.CombatComponent)
            {
                EntityCombatComponent = CombatComponentFactory.GetCombatComponent(_config.CombatComponentConfig);
                EntityCombatComponent.Init(this,_config.CombatComponentConfig);
                _entityComponent.Add(EntityCombatComponent);
                statHolders.Add(EntityCombatComponent);
            }
            
            if (_config.AbilityComponent)
            {
                EntityAbilitiesComponent = AbilityComponentFactory.GetAbilitiesComponent(_config.AbilityComponentConfig);
                EntityAbilitiesComponent.Init(this,_config.AbilityComponentConfig);
                _entityComponent.Add(EntityAbilitiesComponent);
                statHolders.Add(EntityAbilitiesComponent);
            }
            
            if (_config.MovementComponent)
            {
                EntityMovementComponent = MovementComponentFactory.GetMovementComponent(_config.MovementComponentConfig);
                EntityMovementComponent.Init(this,_config.MovementComponentConfig,_agentMoveComponent);
                _entityComponent.Add(EntityMovementComponent);
                statHolders.Add(EntityMovementComponent);
            }
            
            if (_config.AIComponent)
            {
                EntityAIComponent = AIComponentFactory.GetAIComponent(_config.AIComponentConfig);
                EntityAIComponent.Init(this,this,_config.AIComponentConfig);
                _entityComponent.Add(EntityAIComponent);
            }
            
            EntityStatComponent = new StatHandlerComponent();//may need to work in init!
            EntityStatComponent.Init(this,statHolders);
            _entityComponent.Add(EntityStatComponent);
            
            EntityStatComponent.OnStatusEffectAdded += AddStatusEffectVisual;
            
            foreach (var stat in EntityStatComponent.GetAllStats())
            {
                stat.OnValueChanged += EntityVisualComponent.PopUpTexter.SpawnPopUp;
#if UNITY_EDITOR
                _stats.Add(stat);
#endif
            }
            
            #region Temp

            if (_config.VisualComponentConfig.HpBar)//Temp!
                EntityHealthComponent.Health.OnValueChanged += _hpBarConnector.SetBarToHealth;

            if (_config.VisualComponentConfig.HpBar)
                _hpBarConnector.Init(this);
            else
                _hpBarConnector.gameObject.SetActive(false);

            #endregion
            
            gameObject.SetActive(true);
            
            IsInitialization = true;
        }
        
        [Obsolete("may need to use UnitEntitySerializeData only")]
        public virtual void Init(UnitEntityConfig parameter)//need to oder logic to many responsibility
        {
            var serializeData = DataManager.DataRequester.GetSerializeData<UnitEntitySerializeData>(parameter);
            
            Init(serializeData);
        }
        
        #endregion    
        
        #region UnityCallBacks
        
        protected override void Update()
        {
            base.Update();

            if (!IsInitialization)
                return;
            
            foreach (var entityComponent in _entityComponent)
                entityComponent?.UpdateComponent();

            if (EntityTargetingComponent.CurrentTarget == null ||
                EntityTargetingComponent.CurrentTarget.EntityHealthComponent.IsEntityDead)
                EntityTargetingComponent.TrySetNewTarget();
        }

        private void OnValidate()
        {
            _soundHandler ??= GetComponentInChildren<SoundHandler>();
            _entityTargetingComponent ??= GetComponentInChildren<TargetingComponent>();
            _colliderTargeting ??= GetComponentInChildren<ColliderTargetingArea>();
            _entityVisualComponent ??= GetComponentInChildren<UnitEntityVisualComponent>();
            _hpBarConnector ??= GetComponentInChildren<TEMP_UNIT_HPBarConnector>();
            _agentMoveComponent ??= GetComponent<AgentMoveComponent>();
        }

        private void OnDrawGizmosSelected()
        {
            if (EntityTargetingComponent != null)
            {
                if (EntityTargetingComponent.CurrentTarget == null) return;
                
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position,EntityTargetingComponent.CurrentTarget.GameEntity.transform.position);
                Gizmos.color = Color.white;
            }
        }

        protected virtual void OnDestroy()
        {
            if (!IsInitialization)
                return;

            EntityStatComponent.OnStatusEffectAdded -= AddStatusEffectVisual;

            EntityHealthComponent.Health.OnValueChanged -= _hpBarConnector.SetBarToHealth;

            foreach (var stat in EntityStatComponent.GetAllStats())
                stat.OnValueChanged -= EntityVisualComponent.PopUpTexter.SpawnPopUp;
        }

        #endregion
        
        #region VisualComponent
        
        private void AddStatusEffectVisual(EffectSequenceConfig effectSequenceConfig) =>
            EntityVisualComponent.EffectSequenceHandler.PlaySequenceByData(effectSequenceConfig);//temp
        
        
        #endregion

        public void Free()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            EntityMovementComponent?.Dispose();
            OnDispose?.Invoke(this);
        }
    }
}