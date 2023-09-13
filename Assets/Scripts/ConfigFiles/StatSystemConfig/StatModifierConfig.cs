using System;
using Tzipory.GameplayLogic.StatusEffectTypes;
using UnityEngine;

namespace Tzipory.GameplayLogic.StatusEffectTypes
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