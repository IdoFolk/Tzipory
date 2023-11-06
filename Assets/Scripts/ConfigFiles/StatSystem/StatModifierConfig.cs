using System;
using Sirenix.OdinInspector;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
{
    [Serializable]
    public struct StatModifierConfig
    {
        [SerializeField] private StatusModifierType _statusModifierType;
        [SerializeField, HideIf("_enableRingModifiers")] public float Modifier;
        [SerializeField, ShowIf("_enableRingModifiers")] private float[] _ringModifiers;
        [SerializeField] private bool _enableRingModifiers;

        public StatusModifierType StatusModifierType => _statusModifierType;

        public float[] RingModifiers => _ringModifiers;

        public void ChangeRingModifiersNumber(int ringAmount)
        {
            if (_enableRingModifiers)
            {
                _ringModifiers = new float[ringAmount];
            }
        }
    }
}