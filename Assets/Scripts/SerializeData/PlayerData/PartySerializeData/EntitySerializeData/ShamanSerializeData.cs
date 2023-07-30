using Shamans;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace Tzipory.SerializeData.ShamanSerializeData
{
    [System.Serializable]
    public class ShamanSerializeData : UnitEntitySerializeData , IUpdateData<Shaman>
    {
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanId;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanLevel;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanExp;
        
        //add Item serializeData
        //add consumables serializeData
        
        public int ShamanId => _shamanId;
        public int ShamanLevel => _shamanLevel;
        public int ShamanExp => _shamanExp;

        public bool IsInitialization { get; }
        
        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            var config = (ShamanConfig)parameter;
        }

        public void UpdateData(Shaman data)
        {
            throw new System.NotImplementedException();
        }
    }
}