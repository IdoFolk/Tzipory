using Helpers.Consts;
using SerializeData.StatSerializeData;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [System.Serializable]
    public class StatEffectConfig : IConfigFile
    {
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _statusEffectName;
        [SerializeField, Tooltip("")] private int _statProcessPriority;
        [SerializeField, Tooltip("")] private Constant.StatsId _affectedStatId;
        [Header("Stat Modifier")] 
        [SerializeField, Tooltip("")] private StatEffectType _statEffectType;
        [SerializeField, Tooltip(""),ShowIf("ShowDuration")] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statEffectType",StatEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip("")] private StatModifierConfig _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;//may need to change

        private bool ShowDuration =>
            _statEffectType is StatEffectType.OverTime or StatEffectType.Interval; 
        
        
        public string StatusEffectName => _statusEffectName;
        public int StatProcessPriority => _statProcessPriority;
        public float Duration => _duration;
        public float Interval => _interval;
        public int AffectedStatId => (int)_affectedStatId;
        public StatEffectType StatEffectType => _statEffectType;
        public StatModifierConfig StatModifier => _statModifier;
        public EffectSequenceConfig EffectSequence => _effectSequence;
        
        public int ConfigObjectId { get; }
        
        public int ConfigTypeId { get; }
    }
}