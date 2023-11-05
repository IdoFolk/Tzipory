using System;
using Sirenix.OdinInspector;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
{
    [Serializable]
    public class StatModifierConfig
    {
        [SerializeField] private StatusModifierType _statusModifierType;
        [SerializeField, HideIf("_enableRingModifiers")] private float _modifier;
        [SerializeField, ShowIf("_enableRingModifiers")] private float[] _ringModifiers;
        [SerializeField] private bool _enableRingModifiers;

        public StatusModifierType StatusModifierType => _statusModifierType;

        public float Modifier => _modifier;
        public float[] RingModifiers => _ringModifiers;

        public void ModifyStatEffect(float modifiedValue)
        {
            _modifier = modifiedValue;
        }

        public void ChangeRingModifiersNumber(int ringAmount)
        {
            if (_enableRingModifiers)
            {
                _ringModifiers = new float[ringAmount];
            }
        }
    }
}