using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SerializeData.VisualSystemSerializeData
{
    [Serializable]
    public class EffectSequenceConfig
    {
        [SerializeField,ReadOnly] private string _sequenceName;//need to see if can stay readOnly
        [SerializeField,ReadOnly] private int _id;
        
        [SerializeField] private float _startDelay;
        [SerializeField] private float _endDelay;
        
        [SerializeField] private bool _isStackable;
        [SerializeField] private bool _isInterruptable;
        
        [SerializeField] private List<EffectActionContainerConfig> _effectActionContainers;

        public string SequenceName
        {
            get => _sequenceName;
            set => _sequenceName = value;
        }

        public int ID {get => _id;
            set => _id = value;}

        public float StartDelay => _startDelay;

        public float EndDelay => _endDelay;

        public bool IsInterruptable => _isInterruptable;

        public bool IsStackable => _isStackable;

        public List<EffectActionContainerConfig> EffectActionContainers => _effectActionContainers;
    }
}