using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.SerializeData.CurrencySystem
{
    [System.Serializable]
    public class CurrencySerializeData
    {
        [SerializeField] private Constant.Materials _material;
        [SerializeField] private int _amount;

        public bool TryReduceAmount(int amount)
        {
            if (_amount - amount < 0)
                return false;
            
            return true;
        }
        
        public void ReduceAmount(int amount)=>
            _amount -= amount;
        
        public Constant.Materials Material => _material;

        public int Amount => _amount;
    }
}