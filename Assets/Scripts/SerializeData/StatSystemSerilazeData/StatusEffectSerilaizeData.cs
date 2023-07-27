using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using SerializeData.VisualSystemSerializeData;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

namespace Tzipory.SerializeData.StatSystemSerilazeData
{
    [System.Serializable]
    public class StatusEffectSerializeData : ISerializeData
    {
        public int SerializeTypeId => Constant.DataId.StatusEffectDataID;
        
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
    }
}