using System;
using Tzipory.VisualSystem.EffectSequence;
using UnityEngine;

namespace SerializeData.VisualSystemSerializeData
{
    [Serializable]
    public class EffectActionContainerConfig
    {
        [SerializeField] private bool _disableUndo;
        [SerializeField] private float _startDelay;
        [SerializeField] private EffectActionStartType _effectActionStart;
        [SerializeField] private BaseEffectActionConfig _effectActionConfig;

        public bool DisableUndo => _disableUndo;

        public float StartDelay => _startDelay;
        
        public EffectActionStartType EffectActionStart => _effectActionStart;
        
        public BaseEffectActionConfig EffectActionConfig => _effectActionConfig;
    }
}