using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Item;
using Tzipory.ConfigFiles.Player.Inventory;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.ItemSerializeData;
using Tzipory.Systems.DataManager;
using UnityEngine;

namespace Tzipory.SerializeData.Inventory
{
    [System.Serializable]
    public class InventorySerializeData : ISerializeData
    {
        [SerializeField] private List<ItemContainerSerializeData> _itemData;

        public bool IsInitialization { get; private set; }
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

            IsInitialization = true;
        }

        public void AddItemData(int itemId, int amount)
        {
            foreach (var itemContainerSerializeData in _itemData)
            {
                if (itemContainerSerializeData.ItemId == itemId)
                {
                    itemContainerSerializeData.AddItemAmount(amount);
                    return;
                }
            }
            var itemConfig = DataManager.DataRequester.GetConfigData<ItemConfig>(itemId);
            
            ItemContainerSerializeData serializeData = new ItemContainerSerializeData();
            serializeData.Init(itemConfig, amount);
            _itemData.Add(serializeData);
        }

        public int SerializeTypeId => Constant.DataId.INVENTORY_DATA_ID;
    }
}