using System.Collections.Generic;
using Tzipory.Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.SerializeData.StatSystemSerializeData
{
    [System.Serializable]
    public class StatusEffectSerializeData : ISerializeData //need to save ability statEffect(may not be needed)
    {
        public int SerializeTypeId => Constant.DataId.STATUS_EFFECT_DATA_ID;
        
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _StatusEffectName;
        [SerializeField, Tooltip("")] private Constant.StatsId _affectedStatId;
        [SerializeField, Tooltip("")] private List<StatEffectConfig> _statusEffectToInterrupt;
        [Header("Stat Modifier")] 
        [SerializeField, Tooltip("")] private StatEffectType _statEffectType;
        [SerializeField, Tooltip(""),ShowIf("_showDuration")] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statEffectType",StatEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip("")] private StatModifierConfig _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;

        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}