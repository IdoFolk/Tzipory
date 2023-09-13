using Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig;
using UnityEngine;

namespace Tzipory.GameplayLogic.StatusEffectTypes
{
    [System.Serializable]
    public class StatConfig : IConfigFile
    {
        public Constant.StatsId _statsId;
        [SerializeField] private float _baseValue;
        
        public string Name => _statsId.ToString();
        
        public int ID => (int)_statsId;
        public float BaseValue => _baseValue;

        public int ConfigObjectId { get; }
        public int ConfigTypeId => Constant.DataId.STAT_DATA_ID;
    }
}