using System;
using Tzipory.ConfigFiles.VisualSystemConfig;
using UnityEngine;

namespace Tzipory.ConfigFiles.VisualSystemConfig
{
    [Serializable]
    public class StatModifierConfig
    {
        [SerializeField] private StatusModifierType _statusModifierType;
        [SerializeField] private float _modifier;

        public StatusModifierType StatusModifierType => _statusModifierType;

        public float Modifier => _modifier;
    }
}