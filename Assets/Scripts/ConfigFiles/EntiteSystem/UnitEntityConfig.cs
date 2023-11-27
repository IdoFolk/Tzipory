using System;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem
{
    public class UnitEntityConfig : ScriptableObject, IConfigFile
    {
        [SerializeField] private int _entityId;
        [SerializeField] private UnitType _unitType;

        [SerializeField] private HealthComponentConfig _healthComponentConfig;

        [SerializeField] private TargetingComponentConfig _targetingComponentConfig;

        [SerializeField] private VisualComponentConfig _visualComponentConfig;
        
        [SerializeField] private bool _haveCombatComponent;
        [SerializeField,ShowIf(nameof(_haveCombatComponent))] private CombatComponentConfig _combatComponentConfig;
        
        [SerializeField] private bool _haveMovementComponent;
        [SerializeField,ShowIf(nameof(_haveMovementComponent))] private MovementComponentConfig _movementComponentConfig;
        
        [SerializeField] private bool _haveAbilityComponent;
        [SerializeField,ShowIf(nameof(_haveAbilityComponent))] private AbilityComponentConfig _abilityComponentConfig;
        
        public UnitType UnitType => _unitType;

        public HealthComponentConfig HealthComponentConfig => _healthComponentConfig;

        public TargetingComponentConfig TargetingComponentConfig => _targetingComponentConfig;

        public VisualComponentConfig VisualComponentConfig => _visualComponentConfig;

        public CombatComponentConfig CombatComponentConfig => _combatComponentConfig;

        public MovementComponentConfig MovementComponentConfig => _movementComponentConfig;

        public AbilityComponentConfig AbilityComponentConfig => _abilityComponentConfig;

        public bool HaveCombatComponent => _haveCombatComponent;

        public bool HaveMovementComponent => _haveMovementComponent;

        public bool HaveAbilityComponent => _haveAbilityComponent;

        public int ObjectId => _entityId;

        public int ConfigTypeId
        {
            get
            {
                return _unitType switch
                {
                    UnitType.Shaman => Constant.DataId.SHAMAN_DATA_ID,
                    UnitType.Enemy => Constant.DataId.ENEMY_DATA_ID,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

    }

    public enum UnitType
    {
        Shaman,
        Enemy
    }
}