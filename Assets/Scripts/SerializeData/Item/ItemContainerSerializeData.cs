using Helpers.Consts;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Inventory;
using Tzipory.ConfigFiles.Item;
using Tzipory.SerializeData;
using Tzipory.Systems.InventorySystem;
using UnityEngine;

namespace SerializeData.ItemSerializeData
{
    [System.Serializable]
    public class ItemContainerSerializeData : ISerializeData , ISlotItem
    {
        public bool IsInitialization { get; private set; }

        [SerializeField] private int _itemId;
        [SerializeField] private int _itemStack;
        
        private ItemConfig _itemConfig;
        
        public void Init(IConfigFile parameter)
        {
            var config = (ItemContainerConfig)parameter;
            
            _itemConfig = config.ItemConfig;
            
            _itemId = config.ObjectId;
            _itemStack = config.Amount;
            
            IsInitialization = true;
        }

        public int SerializeTypeId => Constant.DataId.ITEM_DATA_ID;
        public Sprite ItemSlotSprite => _itemConfig.ItemIcon;
        public string ItemSlotName => _itemConfig.ItemName;
        public string ItemSlotDescription => _itemConfig.ItemDescription;
        public ItemSlot ItemSlot => _itemConfig.ItemSlot;
        public int ItemId => _itemId;
        public int ItemAmount => _itemStack;
    }
}