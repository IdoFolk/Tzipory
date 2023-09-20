using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.ItemSerializeData;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;
using UnityEngine;

namespace SerializeData.InventorySerializeData
{
    [System.Serializable]
    public class InventorySerializeData : ISerializeData
    {
        public bool IsInitialization { get; }

        [SerializeField] private List<ItemContainerSerializeData> _itemData;
        
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }

        public int SerializeTypeId => Constant.DataId.INVENTORY_DATA_ID;
    }
}