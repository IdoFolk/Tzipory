using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.SerializeData.StatSystemSerializeData
{
    [System.Serializable]
    public class StatSerializeData : ISerializeData , IUpdateData<Stat>
    {
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private int _id;
        [SerializeField,ReadOnly] private float _value;

        public string Name => _name;

        public int ID => _id;

        public float Value => _value;

        public int SerializeObjectId => _id;
        public int SerializeTypeId => Constant.DataId.STAT_DATA_ID;

        public StatSerializeData(StatConfig statConfig)
        {
             _name  = statConfig.Name;
             _id = statConfig.ID;
             _value = 0;
        }

        public StatSerializeData(Stat stat)
        {
            _name  = stat.Name;
            _id = stat.Id;
            _value = stat.DynamicValue;
        }

        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateData(Stat data)
        {
            _value = data.DynamicValue;
        }
    }
}