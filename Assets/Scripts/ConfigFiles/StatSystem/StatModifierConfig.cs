using System;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
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