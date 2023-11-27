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
        [SerializeField,TabGroup("Main config")] private int _entityId;
        [SerializeField,TabGroup("Main config")] private UnitType _unitType;

        [SerializeField,TabGroup("Base Component")] public HealthComponentConfig HealthComponentConfig;

        [SerializeField,TabGroup("Base Component")] public TargetingComponentConfig TargetingComponentConfig;

        [SerializeField,TabGroup("Base Component")] public VisualComponentConfig VisualComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _haveCombatComponent;
        [SerializeField,ShowIf(nameof(_haveCombatComponent)),TabGroup("Optional Component")] public CombatComponentConfig CombatComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _haveMovementComponent;
        [SerializeField,ShowIf(nameof(_haveMovementComponent)),TabGroup("Optional Component")] public MovementComponentConfig MovementComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _haveAbilityComponent;
        [SerializeField,ShowIf(nameof(_haveAbilityComponent)),TabGroup("Optional Component")] public AbilityComponentConfig AbilityComponentConfig;
        
        [SerializeField,TabGroup("Optional Component")] private bool _haveAiComponent;
        [SerializeField,ShowIf(nameof(_haveAiComponent)),TabGroup("Optional Component")] public AIComponentConfig AIComponentConfig;
        
        public UnitType UnitType => _unitType;
        
        public bool HaveAiComponent => _haveAiComponent;

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

        private void OnValidate()
        {
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