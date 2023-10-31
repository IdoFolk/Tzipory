using System;
using Sirenix.OdinInspector;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructureHandler : MonoBehaviour, ITargetableReciever
    {
        [SerializeField] private ProximityCircleManager _proximityCircleManager;
        [SerializeField] private PowerStructureConfig _powerStructureConfig;

        private bool _toggleShowCircles; // TEMP

        private void Awake()
        {
            _proximityCircleManager.Init(_powerStructureConfig, this);
        }

        private void OnValidate()
        {
            if (_powerStructureConfig.RingsRatios.Length != _proximityCircleManager.RingHandlers.Length)
            {
                Debug.LogError("the number of Rings in the SO is different than the actual rings in the prefab");
            }
        }
        
        [Button("ToggleShowCircles")]
        private void ToggleShowRings()
        {
            _toggleShowCircles = !_toggleShowCircles;
            _proximityCircleManager.ToggleShowRings(_powerStructureConfig, _toggleShowCircles);
        }


        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
        }
    }
}