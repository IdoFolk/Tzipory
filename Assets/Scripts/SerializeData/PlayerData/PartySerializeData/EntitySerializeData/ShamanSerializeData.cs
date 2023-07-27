using Shamans;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace Tzipory.SerializeData.ShamanSerializeData
{
    [System.Serializable]
    public class ShamanSerializeData : UnitEntitySerializeData
    {
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanId;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanLevel;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanExp;
        
        //add Item serializeData
        //add consumables serializeData
        
        public int ShamanId => _shamanId;

        public int ShamanLevel => _shamanLevel;
        public int ShamanExp => _shamanExp;
        
        
        public ShamanSerializeData(ShamanConfig shamanConfig) : base(shamanConfig)
        {
        }

        public ShamanSerializeData(Shaman shaman) : base(shaman)
        {
        }
    }
}