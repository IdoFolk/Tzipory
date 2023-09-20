using System;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

namespace SerializeData.StatSerializeData
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