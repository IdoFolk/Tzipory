using System;
using Sirenix.OdinInspector;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
{
    [Serializable]
    public struct StatModifierConfig
    {
        [SerializeField] private StatModifierType _statModifierType;
        [SerializeField, HideIf("_enableRingModifiers")] public float Modifier;
        [SerializeField, ShowIf("_enableRingModifiers")] public float[] _ringModifiers;
        private bool _enableRingModifiers;

        public StatModifierType StatModifierType => _statModifierType;

        public float[] RingModifiers => _ringModifiers;

        public void ChangeRingModifiersNumber(int ringAmount)
        {
            if (_enableRingModifiers)
            {
                _ringModifiers = new float[ringAmount];
            }
        }

        public void ToggleRingModifiers(bool state)
        {
            _enableRingModifiers = state;
        }
    }
}