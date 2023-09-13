using System.Collections.Generic;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.GameplayLogic.StatusEffectTypes;
using UnityEngine;

namespace Tzipory.ConfigFiles.WaveSystemConfig.StatSystemSerilazeData
{
    [System.Serializable]
    public class StatusEffectSerializeData : ISerializeData
    {
        public int SerializeTypeId => Constant.DataId.STATUS_EFFECT_DATA_ID;
        
        [Header("Status Effect Config")] 
        [SerializeField, Tooltip("")] private string _StatusEffectName;
        [SerializeField, Tooltip("")] private Constant.StatsId _affectedStatId;
        [SerializeField, Tooltip("")] private List<StatusEffectConfig> _statusEffectToInterrupt;
        [Header("Stat Modifiers")] 
        [SerializeField, Tooltip("")] private StatusEffectType _statusEffectType;
        [SerializeField, Tooltip(""),ShowIf("_showDuration")] private float _duration;
        [SerializeField, Tooltip(""),ShowIf("_statusEffectType",StatusEffectType.Interval)] private float _interval;
        [SerializeField, Tooltip("")] private List<StatModifierConfig> _statModifier;
        [Header("Status effect visual")]
        [SerializeField, Tooltip("")] private EffectSequenceConfig _effectSequence;

        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}