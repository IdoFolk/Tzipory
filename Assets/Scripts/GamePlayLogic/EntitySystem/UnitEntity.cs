using System;
using System.Collections.Generic;
using GameplayLogic.UI.HPBar;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.GamePlayLogic.EntitySystem.EntityComponent;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.FactorySystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using UnityEngine;

namespace Tzipory.GamePlayLogic.EntitySystem
{
    public class UnitEntity : BaseGameEntity, ITargetAbleEntity, IInitialization<UnitEntityConfig> , IInitialization<UnitEntitySerializeData>
    {
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
        [Header("Visual components")]
        [SerializeField,TabGroup("Component")] private UnitEntityVisualComponent _entityVisualComponent;//temp

        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onDeath;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onCritAttack;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onMove;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onGetHit;
        [SerializeField,TabGroup("Visual Events")] private EffectSequenceConfig _onGetCritHit;
        
        #region Temps
        
        [Header("Temps")] 
        [SerializeField] private Transform _shotPosition;
        [SerializeField] private bool _doShowHPBar;
        [SerializeField] private TEMP_UNIT_HPBarConnector _hpBarConnector;
        
        #endregion

        private IEntityComponent[] _entityComponent;
        
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
            
            _entityVisualComponent.Init(this,effectSequence);
            EntityTargetingComponent.Init(this,_colliderTargeting,_config.TargetingComponentConfig);
            
            EntityHealthComponent = HealthComponentFactory.GetHealthComponent(_config.HealthComponentConfig);
            EntityHealthComponent.Init(this,_config.HealthComponentConfig);

            if (_config.HaveCombatComponent)
            {
                EntityCombatComponent = CombatComponentFactory.GetCombatComponent(_config.CombatComponentConfig);
                EntityCombatComponent.Init(this,_config.CombatComponentConfig);
            }
            
            if (_config.HaveAbilityComponent)
            {
                EntityAbilitiesComponent = AbilityComponentFactory.GetAbilitiesComponent(_config.AbilityComponentConfig);
                EntityAbilitiesComponent.Init(this,_config.AbilityComponentConfig);
            }
            
            if (_config.HaveMovementComponent)
            {
                EntityMovementComponent = MovementComponentFactory.GetMovementComponent(_config.MovementComponentConfig);
                EntityMovementComponent.Init(this,_config.MovementComponentConfig);
            }

            IStatHolder[] statHolders = {
                EntityHealthComponent,
                EntityTargetingComponent,
                EntityMovementComponent,
                EntityAbilitiesComponent,
                EntityCombatComponent
            };
            
            EntityStatComponent = new StatHandlerComponent(this,statHolders);//may need to work in init!
            
            EntityStatComponent.OnStatusEffectAdded += AddStatusEffectVisual;
            
            foreach (var stat in EntityStatComponent.GetAllStats())
            {
                stat.OnValueChanged += EntityVisualComponent.PopUpTexter.SpawnPopUp;
#if UNITY_EDITOR
                _stats.Add(stat);
#endif
            }
            
            _entityComponent = new IEntityComponent[]
            {
                EntityTargetingComponent,
                EntityMovementComponent,
                EntityAbilitiesComponent,
                EntityHealthComponent,
                EntityStatComponent,
                EntityCombatComponent
            };
            
            SetSprite(_config.VisualComponentConfig.Sprite);

            #region Temp

            if (_doShowHPBar)//Temp!
                EntityHealthComponent.Health.OnValueChanged += _hpBarConnector.SetBarToHealth;

            if (_doShowHPBar)
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
            var serializeData = DataManager.DataRequester.GetSerializeData<UnitEntitySerializeData>(parameter.ObjectId);
            
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

        private void SetSprite(Sprite newSprite)
        {
            EntityVisualComponent.SpriteRenderer.sprite = newSprite;
            OnSetSprite?.Invoke(newSprite);
        }
        public void SetSpriteFlipX(bool doFlip)
        {
            EntityVisualComponent.SpriteRenderer.flipX = doFlip;
            OnSpriteFlipX?.Invoke(doFlip);
        }
        
        #endregion
    }
}