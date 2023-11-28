using System;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem
{
    [CreateAssetMenu(fileName = "UnitEntityConfig", menuName = "ScriptableObjects/Entity/NewEntity")]
    public class UnitEntityConfig : ScriptableObject, IConfigFile
    {
        [SerializeField,ReadOnly] private int _entityId;
        [SerializeField,TabGroup("Main config")] private UnitType _unitType;

        [SerializeField,TabGroup("Base Component")] public HealthComponentConfig HealthComponentConfig;

        [SerializeField,TabGroup("Base Component")] public TargetingComponentConfig TargetingComponentConfig;

        [SerializeField,TabGroup("Base Component")] public VisualComponentConfig VisualComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _combatComponent;
        [SerializeField,ShowIf(nameof(_combatComponent)),TabGroup("Optional Component")] public CombatComponentConfig CombatComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _movementComponent;
        [SerializeField,ShowIf(nameof(_movementComponent)),TabGroup("Optional Component")] public MovementComponentConfig MovementComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _abilityComponent;
        [SerializeField,ShowIf(nameof(_abilityComponent)),TabGroup("Optional Component")] public AbilityComponentConfig AbilityComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _aiComponent;
        [SerializeField,ShowIf(nameof(_aiComponent)),TabGroup("Optional Component")] public AIComponentConfig AIComponentConfig;
        
        public UnitType UnitType => _unitType;
        
        public bool AIComponent => _aiComponent;

        public bool CombatComponent => _combatComponent;

        public bool MovementComponent => _movementComponent;

        public bool AbilityComponent => _abilityComponent;

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

        private void OnValidate()
        {
            _entityId = name.GetHashCode();
            
            if (_unitType == UnitType.Shaman)
            {
                TargetingComponentConfig.EntityType = EntityType.Hero;
                TargetingComponentConfig.TargetedEntity = EntityType.Enemy;
                
                AIComponentConfig.AIType = AIComponentType.Hero;
            }
            
            if (_unitType == UnitType.Enemy)
            {
                TargetingComponentConfig.EntityType = EntityType.Enemy;
                TargetingComponentConfig.TargetedEntity = EntityType.Hero | EntityType.Core;

                AIComponentConfig.AIType = AIComponentType.Enemy;
            }
        }
    }

    public enum UnitType
    {
        Shaman,
        Enemy
    }
}