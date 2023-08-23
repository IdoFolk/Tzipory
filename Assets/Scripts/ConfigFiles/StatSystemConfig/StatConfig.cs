using Helpers.Consts;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace SerializeData.StatSerializeData
{
    [System.Serializable]
    public class StatConfig : IConfigFile
    {
        public Constant.Stats _stats;
        [SerializeField] private float _baseValue;
        
        public string Name => _stats.ToString();
        
        public int ID => (int)_stats;
        public float BaseValue => _baseValue;

        public int ConfigObjectId { get; }
        public int ConfigTypeId => Constant.DataId.STAT_DATA_ID;
    }
}