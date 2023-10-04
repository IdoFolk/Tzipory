using Tzipory.ConfigFiles;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public class PlayerCurrencyConfig : IConfigFile
    {
        [SerializeField] private CurrencyContainer[] _currency;

        public CurrencyContainer[] Currency => _currency;

        public int ObjectId { get; }
        public int ConfigTypeId { get; }
    }
    
    [System.Serializable]
    public class CurrencyContainer
    {
        [SerializeField] private Constant.Materials _material;
        [SerializeField] private int _amount;

        public Constant.Materials Material => _material;

        public int Amount => _amount;
    }
}