using Tzipory.GameplayLogic.UI.ProximityIndicators;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.MovementSystem.HerosMovementSystem;
using Tzipory.Tools.TimeSystem;
using Tzipory.Helpers;
using Sirenix.OdinInspector;
using Spine;
using Tzipory.ConfigFiles.EntitySystem.EntityVisual;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.AnimationSystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;
using Event = Spine.Event;

namespace Tzipory.GameplayLogic.EntitySystem.Shamans
{
    public class Shaman : BaseUnitEntity
    {
        private const string BASIC_ATTACK_EVENT = "Basic Attack Event";
        private const string CRIT_ATTACK_EVENT = "Crit Attack Event";
        private const string AOE_ATTACK_EVENT = "AOE Attack Event";

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

        public override void Init(UnitEntitySerializeData parameter, BaseUnitEntityVisualConfig visualConfig)
        {
            base.Init(parameter, visualConfig);
            var shamanSerializeData = (ShamanSerializeData)parameter;
            _serializeData = shamanSerializeData;
            
            _shotVisual.Init(this);

            _baseDecisionInterval = shamanSerializeData.DecisionInterval;

            EntityType = EntityType.Hero;
            _clickHelper.OnClick += _tempHeroMovement.SelectHero;
            
            _proximityHandler.Init(AttackRange.CurrentValue);//MAY need to move to OnEnable - especially if we use ObjectPooling instead of instantiate
            
            AnimationHandler.AnimationState.Event += OnAnimationEventRecieve;
        }

        private void OnDisable()
        {
            _proximityHandler.Disable();
        }

        protected override void UpdateEntity()
        {
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
            
            if (_currentDecisionInterval < 0)
            {
                TargetingHandler.GetPriorityTarget();
                _currentDecisionInterval = _baseDecisionInterval;
            }
            
            if (TargetingHandler.CurrentTarget != null)//temp
                Attack();

            if (_tempHeroMovement.IsMoveing && AnimationHandler.gameObject.activeSelf)
            {
                if (AnimationHandler.CurrentAnimationStateType != AnimationStates.Running)
                    AnimationHandler.TEMP_SetAnimation(AnimationStates.Running);
            }
            else
            {
                if (AnimationHandler.CurrentAnimationStateType != AnimationStates.Idle)
                    AnimationHandler.TEMP_SetAnimation(AnimationStates.Idle);
            }
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
            
            AbilityHandler.CastAbility(TargetingHandler.AvailableTargets);
            
            
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

            if (AnimationHandler.gameObject.activeSelf)
            {
                if (CritChance.CurrentValue > Random.Range(0, 100))
                {
                    EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);
                    AnimationHandler.TEMP_SetAnimation(AnimationStates.CritAttack);
                    return;
                }
                EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
                AnimationHandler.TEMP_SetAnimation(AnimationStates.BasicAttack);
            }
            else
            {
                if (CritChance.CurrentValue > Random.Range(0, 100))
                {
                    EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);
                    _shotVisual.Shot(TargetingHandler.CurrentTarget,AttackDamage.CurrentValue * (CritDamage.CurrentValue / 100),true);
                    return;
                }
                EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
                _shotVisual.Shot(TargetingHandler.CurrentTarget,AttackDamage.CurrentValue,false);
            }
           
            
        }
        private void OnAnimationEventRecieve(TrackEntry trackentry, Event e)
        {
            switch (e.Data.Name)
            {
                case BASIC_ATTACK_EVENT:
                    _shotVisual.Shot(TargetingHandler.CurrentTarget,AttackDamage.CurrentValue,false);
                    Debug.Log("Shots Fired");
                    break;
                case CRIT_ATTACK_EVENT:
                    _shotVisual.Shot(TargetingHandler.CurrentTarget,AttackDamage.CurrentValue * (CritDamage.CurrentValue / 100),true);
                    Debug.Log("Shots Fired");
                    break;
                case AOE_ATTACK_EVENT:
                    //Cast Ability projectile
                    Debug.Log("Ability Used");
                    break;
            }
        }
        protected override void EntityDied()
        {
            base.EntityDied();
            Debug.Log($"{gameObject.name} as Died!");
            gameObject.SetActive(false);
        }
    }
}