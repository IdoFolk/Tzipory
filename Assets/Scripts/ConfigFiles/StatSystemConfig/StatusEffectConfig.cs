using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [System.Serializable]
    public class StatusEffectConfig : IConfigFile
    {
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _statusEffectName;
        [SerializeField, Tooltip("")] private Constant.Stats _affectedStat;
        //[SerializeField, Tooltip("")] private List<StatusEffectConfig> _statusEffectToInterrupt;
        [Header("Stat Modifiers")] 
        [SerializeField, Tooltip("")] private StatusEffectType _statusEffectType;
        [SerializeField, Tooltip(""),HideIf("_statusEffectType",StatusEffectType.Instant)] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statusEffectType",StatusEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip("")] private List<StatModifierConfig> _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;

        public string StatusEffectName => _statusEffectName;

        public float Duration => _duration;

        public float Interval => _interval;

        public string AffectedStatName => _affectedStat.ToString();
        public int AffectedStatId => (int)_affectedStat;
        public StatusEffectType StatusEffectType => _statusEffectType;

        public List<StatModifierConfig> StatModifier => _statModifier;

        //public List<StatusEffectConfig> StatusEffectToInterrupt => _statusEffectToInterrupt;
        
        public EffectSequenceConfig EffectSequence => _effectSequence;
        public int ConfigObjectId { get; }
        
        public int ConfigTypeId { get; }
    }
}