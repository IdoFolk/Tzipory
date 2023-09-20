using System;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.StatusSystem.Stats;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem
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