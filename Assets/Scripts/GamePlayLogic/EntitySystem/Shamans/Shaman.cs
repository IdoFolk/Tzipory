using Helpers.Consts;
using MovementSystem.HerosMovementSystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.Entitys;
using Tzipory.Helpers;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig;
using Tzipory.SerializeData;
using UnityEngine;

namespace Shamans
{
    public class Shaman : BaseUnitEntity
    {
        [SerializeField, TabGroup("Proximity Indicator")] private ProximityIndicatorHandler _proximityHandler;
        [SerializeField,TabGroup("Component")] private ClickHelper _clickHelper;

        [Space]
        [Header("Temps")]
        [SerializeField] private Temp_ShamanShotVisual _shotVisual;
        [SerializeField] private Temp_HeroMovement _tempHeroMovement;
        
        private ShamanSerializeData  _serializeData;
        
        private float _currentAttackRate;

        public override void Init(UnitEntitySerializeData parameter, BaseUnitEntityVisualConfig visualConfig)
        {
            base.Init(parameter, visualConfig);
            var config = (ShamanSerializeData)parameter;
            //add shaman config
            
            EntityTeamType = EntityTeamType.Hero;
            _clickHelper.OnClick += _tempHeroMovement.SelectHero;
            
            _proximityHandler.Init(AttackRange.CurrentValue);//MAY need to move to OnEnable - especially if we use ObjectPooling instead of instantiate
        }

        // public void Init(ShamanSerializeData parameter)
        // {
        //     
        // }

        private void OnDisable()
        {
            _proximityHandler.Disable();
        }

        protected override void UpdateEntity()
        {
            if (Targeting.CurrentTarget != null)//temp
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
            
            AbilityHandler.CastAbility(Targeting.AvailableTargets);
            
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
                EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.CRIT_ATTACK);
                _shotVisual.Shot(Targeting.CurrentTarget,CritDamage.CurrentValue,true);
                return;
            }
            EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.ATTACK);
            _shotVisual.Shot(Targeting.CurrentTarget,AttackDamage.CurrentValue,false);
        }

        public override void OnEntityDead()
        {
            Debug.Log($"{gameObject.name} as Died!");
            gameObject.SetActive(false);
        }
    }
}