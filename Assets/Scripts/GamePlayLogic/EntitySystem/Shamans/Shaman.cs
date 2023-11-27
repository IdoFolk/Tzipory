﻿using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem.EntityVisual;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.GameplayLogic.UI.ProximityIndicators;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.MovementSystem.HerosMovementSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.Shamans
{
    public class Shaman : UnitEntity
    {
        [SerializeField, TabGroup("Proximity Indicator")] private ProximityIndicatorHandler _proximityHandler;
        [SerializeField,TabGroup("Component")] private ClickHelper _clickHelper;

        [Space]
        [Header("Temps")]
        [SerializeField] private Temp_ShamanShotVisual _shotVisual;
        [SerializeField] private Temp_HeroMovement _tempHeroMovement;
        
        private ShamanSerializeData  _serializeData;

        private float _currentDecisionInterval = 0;//temp
        private float _baseDecisionInterval;//temp
        
        private float _currentAttackRate;
        public BaseUnitEntityVisualConfig  VisualConfig { get; private set; } //temp
        
        private IObjectDisposable _uiIndicator;

        public override void Init(UnitEntitySerializeData parameter, BaseUnitEntityVisualConfig visualConfig)
        {
            base.Init(parameter, visualConfig);
            VisualConfig = visualConfig;
            var shamanSerializeData = (ShamanSerializeData)parameter;
            _serializeData = shamanSerializeData;
            
            _shotVisual.Init(_shotVisual.transform);

            _baseDecisionInterval = shamanSerializeData.DecisionInterval;

            EntityType = EntityType.Hero;
            _clickHelper.OnClick += _tempHeroMovement.SelectHero;
            
            _proximityHandler.Init(AttackRange.CurrentValue);//MAY need to move to OnEnable - especially if we use ObjectPooling instead of instantiate
            
            _uiIndicator = UIIndicatorHandler.SetNewIndicator(transform, new UIIndicatorConfig()
            {
                Image = visualConfig.Icon,
                Color = Color.white,
                AllwaysShow = false,
                DisposOnClick = false,
                OffSetRadios = 25,
                StartFlashing = false,
                FlashConfig = new UIIndicatorFlashConfig()
                {
                    SizeFactor = 1.2f,
                    FlashSpeed = .8f,
                    UseTime = true,
                    Time = 3,
                    OverrideFlashingColor = true,
                    FlashingColor = Color.red
                }
            },null,GoToShaman);
        }
        
        private void GoToShaman()=>
            GameManager.CameraHandler.SetCameraPosition(transform.position);

        private void OnDisable()
        {
            _proximityHandler.Disable();
        }

        protected override void UpdateEntity()
        {
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
            
            if (_currentDecisionInterval < 0)
            {
                EntityTargetingComponent.GetPriorityTarget();
                _currentDecisionInterval = _baseDecisionInterval;
            }

            if (EntityTargetingComponent.CurrentTarget != null)//temp
                Attack();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _serializeData?.UpdateData(this);//need to make endGame logic
            _clickHelper.OnClick -= _tempHeroMovement.SelectHero;
        }

        public override void Attack()
        {
            if (_tempHeroMovement.IsMoveing)
            {
                AbilityHandler.CancelCast();
                return;
            }
            
            AbilityHandler.CastAbility(EntityTargetingComponent.AvailableTargets);
            
            bool canAttack = false;

            if (AbilityHandler.IsCasting)//temp!!!
            {
                _currentAttackRate = AttackRate.CurrentValue;
                return;
            }
            
            _currentAttackRate -= GAME_TIME.GameDeltaTime;
            
            if (_currentAttackRate < 0)
            {
                _currentAttackRate = AttackRate.CurrentValue;
                canAttack = true;
            }
            
            if(!canAttack)
                return;
            
            if (CritChance.CurrentValue > Random.Range(0, 100))
            {
                EntityVisualComponent.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);
                _shotVisual.Shot(EntityTargetingComponent.CurrentTarget,AttackDamage.CurrentValue * (CritDamage.CurrentValue / 100),true);
                return;
            }
            
            EntityVisualComponent.EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
            _shotVisual.Shot(EntityTargetingComponent.CurrentTarget,AttackDamage.CurrentValue,false);
        }

        public override void TakeDamage(float damage, bool isCrit)
        {
            base.TakeDamage(damage, isCrit);
            UIIndicatorHandler.StartFlashOnIndicator(_uiIndicator.ObjectInstanceId);
        }

        protected override void EntityDied()
        {
            base.EntityDied();
            Debug.Log($"{gameObject.name} as Died!");
            gameObject.SetActive(false);
        }
    }
}