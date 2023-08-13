using Helpers.Consts;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace SerializeData.StatSerializeData
{
    [System.Serializable]
    public class StatConfig : IConfigFile
    {
        [SerializeField] private string _name;
        [SerializeField] private int _id;
        [SerializeField] private float _baseValue;

        public string Name
        {
            get => _name;
#if UNITY_EDITOR
            set => _name = value;
#endif
        }
        public int Id
        {
            get => _id;
#if UNITY_EDITOR
            set => _id = value;
#endif
        }
        public float BaseValue => _baseValue;

        public int ConfigObjectId { get; }
        public int ConfigTypeId => Constant.DataId.STAT_DATA_ID;
    }
}