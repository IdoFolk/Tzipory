using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
{
    [System.Serializable]
    public class StatEffectConfig : IConfigFile
    {
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _statusEffectName;
        [SerializeField, Tooltip("")] private int _statProcessPriority;
        [SerializeField, Tooltip("")] private Constant.StatsId _affectedStatId;
        [Space]
        [SerializeField, Tooltip(""),TabGroup("Stat Modifier")] private StatEffectType _statEffectType;
        [SerializeField, Tooltip(""),ShowIf("ShowDuration"),TabGroup("Stat Modifier")] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statEffectType",StatEffectType.Interval),TabGroup("Stat Modifier")] private float _interval;
        [SerializeField, Tooltip(""),TabGroup("Stat Modifier")] public StatModifierConfig StatModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip(""),TabGroup("Visual config")] private EffectSequenceConfig _effectSequence;//may need to change

        [SerializeField,TabGroup("Visual config")] private bool _usePopUpTextConfig;
        [SerializeField, Tooltip(""),ShowIf("_usePopUpTextConfig"),TabGroup("Visual config")] private PopUpTextConfig _popUpTextConfig;//may need to change

        private bool ShowDuration =>
            _statEffectType is StatEffectType.OverTime or StatEffectType.Interval; 
        
        public string StatusEffectName => _statusEffectName;
        public int StatProcessPriority => _statProcessPriority;
        public float Duration => _duration;
        public float Interval => _interval;
        public int AffectedStatId => (int)_affectedStatId;
        public Constant.StatsId AffectedStatType => _affectedStatId;
        public StatEffectType StatEffectType => _statEffectType;
        public EffectSequenceConfig EffectSequence => _effectSequence;
        public bool UsePopUpTextConfig => _usePopUpTextConfig;
        public PopUpTextConfig PopUpTextConfig => _popUpTextConfig;
        
        public int ObjectId { get; }
        
        public int ConfigTypeId { get; }
    }
}