using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.SerializeData.CurrencySystem
{
    [System.Serializable]
    public class PlayerCurrencySerializeData
    {
        [SerializeField] private CurrencySerializeData[] _currencySerializeData;
        
        public CurrencySerializeData[] CurrencySerializeData => _currencySerializeData;
        
        public bool TryBuyItem(IEnumerable<CurrencySerializeData> purchaseBill,out PurchaseOder purchaseOder) 
        {
            foreach (var currencySerializeData in purchaseBill)
            {
                foreach (var currencyData in _currencySerializeData)
                {
                    if (currencySerializeData.Material == currencyData.Material)
                    {
                        if (!currencyData.TryReduceAmount(currencySerializeData.Amount))
                        {
                            purchaseOder = default;
                            return false;
                        }
                    }
                }
            }
            
            purchaseOder = default;
            return false;
        }
    }

    public struct PurchaseOder
    {
        public CurrencySerializeData[] CurrencySerializeData;

        public PurchaseOder(CurrencySerializeData[] currencySerializeData)
        {
            CurrencySerializeData = currencySerializeData;
        }
    }
}