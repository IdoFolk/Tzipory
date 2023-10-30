using System;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructureHandler : MonoBehaviour
    {
        [SerializeField] private ProximityCirclesManager _proximityCirclesManager;
        [SerializeField] private PowerStructureSO _powerStructureSO;
        

        private void OnValidate()
        {
            //_proximityCirclesManager.ScaleCircles();
        }
    }
}