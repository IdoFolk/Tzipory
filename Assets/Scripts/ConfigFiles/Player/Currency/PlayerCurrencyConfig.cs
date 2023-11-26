using Tzipory.SerializeData.CurrencySystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.Player.Currency
{
    [System.Serializable]
    public class PlayerCurrencyConfig 
    {
        [SerializeField] private CurrencySerializeData[] _currency;

        public CurrencySerializeData[] Currency => _currency;
    }
}