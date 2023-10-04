using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.StatusSystem
{
    [System.Serializable]
    public class StatConfig : IConfigFile
    {
        public Constant.StatsId _statsId;
        [SerializeField] private float _baseValue;
        
        public string Name => _statsId.ToString();
        
        public int ID => (int)_statsId;
        public float BaseValue => _baseValue;

        public int ObjectId { get; }
        public int ConfigTypeId => Constant.DataId.STAT_DATA_ID;
    }
}