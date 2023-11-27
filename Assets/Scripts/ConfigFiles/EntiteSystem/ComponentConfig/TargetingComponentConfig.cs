using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [Serializable]
    public struct TargetingComponentConfig
    {
        [SerializeField] public float TargetingRange;
        [SerializeField] public EntityType EntityType;
        [SerializeField] public EntityType TargetedEntity;
        [SerializeField] public TargetingPriorityType TargetingPriorityType;
        
    }
}