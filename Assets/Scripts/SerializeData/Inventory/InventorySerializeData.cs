using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Inventory;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.ItemSerializeData;
using UnityEngine;

namespace Tzipory.SerializeData.Inventory
{
    [System.Serializable]
    public class InventorySerializeData : ISerializeData
    {
        public bool IsInitialization { get; }

        [SerializeField] private List<ItemContainerSerializeData> _itemData;

        public List<ItemContainerSerializeData> ItemData => _itemData;

        public void Init(IConfigFile parameter)
        {
            var config = (InventoryConfig)parameter;

            _itemData = new List<ItemContainerSerializeData>();
            
            foreach (var itemContainerConfig in config.Items)
            {
                //ItemContainerSerializeData serializeData = DataManager.DataRequester.GetSerializeData<ItemContainerSerializeData>(itemContainerConfig.ObjectId);
                ItemContainerSerializeData serializeData = new ItemContainerSerializeData();
                serializeData.Init(itemContainerConfig);
                _itemData.Add(serializeData);
            }
        }

        public int SerializeTypeId => Constant.DataId.INVENTORY_DATA_ID;
    }
}