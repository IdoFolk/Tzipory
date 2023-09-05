using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tools.Enums;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [System.Serializable]
    public class StatusEffectConfig : IConfigFile
    {
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _statusEffectName;
        [SerializeField, Tooltip("")] private Constant.StatsId _affectedStatId;
        [SerializeField, Tooltip("")] private EffectType _effectType;
        //[SerializeField, Tooltip("")] private List<StatusEffectConfig> _statusEffectToInterrupt;
        [Header("Stat Modifiers")] 
        [SerializeField, Tooltip("")] private StatusEffectType _statusEffectType;
        [SerializeField, Tooltip(""),HideIf("_statusEffectType",StatusEffectType.Instant)] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statusEffectType",StatusEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip(""),ShowIf("_statusEffectType",StatusEffectType.OverTime)] private bool _disableDuration;
        [SerializeField, Tooltip("")] private List<StatModifierConfig> _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;

        public string StatusEffectName => _statusEffectName;

        public float Duration => _duration;

        public float Interval => _interval;
        
        public EffectType EffectType => _effectType;

        public int AffectedStatId => (int)_affectedStatId;
        public StatusEffectType StatusEffectType => _statusEffectType;

        public bool DisableDuration => _disableDuration;

        public List<StatModifierConfig> StatModifier => _statModifier;
        
        public EffectSequenceConfig EffectSequence => _effectSequence;
        public int ConfigObjectId { get; }
        
        public int ConfigTypeId { get; }
    }
}