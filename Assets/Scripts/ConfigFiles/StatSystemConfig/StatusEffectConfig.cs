using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.EntitySystem.StatusSystem
{
    [CreateAssetMenu(fileName = "NewStatusEffectConfig", menuName = "ScriptableObjects/EntitySystem/StatusSystem/New status effect config", order = 0)]
    public class StatusEffectConfig : ScriptableObject
    {
        private bool _showDuration => _statusEffectType is StatusEffectType.OverTime or StatusEffectType.Interval;

        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _StatusEffectName;
        [SerializeField, Tooltip("")] private Constant.Stats _affectedStat;
        [SerializeField, Tooltip("")] private List<StatusEffectConfig> _statusEffectToInterrupt;
        [Header("Stat Modifiers")] 
        [SerializeField, Tooltip("")] private StatusEffectType _statusEffectType;
        [SerializeField, Tooltip(""),ShowIf("_showDuration")] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statusEffectType",StatusEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip("")] private List<StatModifierConfig> _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;

        public string StatusEffectName => _StatusEffectName;

        public float Duration => _duration;

        public float Interval => _interval;

        public string AffectedStatName => _affectedStat.ToString();
        public int AffectedStatId => (int)_affectedStat;
        public StatusEffectType StatusEffectType => _statusEffectType;

        public List<StatModifierConfig> StatModifier => _statModifier;

        public List<StatusEffectConfig> StatusEffectToInterrupt => _statusEffectToInterrupt;


        public EffectSequenceConfig EffectSequence => _effectSequence;
    }
}