using UnityEngine;

namespace Tzipory.SerializeData.CurrencySystem
{
    [System.Serializable]
    public class PlayerCurrencySerializeData
    {
        [SerializeField] private CurrencySerializeData[] _currencySerializeData;
        
        public CurrencySerializeData[] CurrencySerializeData => _currencySerializeData;
        
        public bool TryBuyItem(CurrencySerializeData[] purchaseBill,out PurchaseOder purchaseOder) 
        {
            foreach (var currencySerializeData in purchaseBill)
            {
                for (int i = 0; i < _currencySerializeData.Length; i++)
                {
                    if (currencySerializeData.Material == _currencySerializeData[i].Material)
                    {
                        if (!_currencySerializeData[i].TryReduceAmount(currencySerializeData.Amount))
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