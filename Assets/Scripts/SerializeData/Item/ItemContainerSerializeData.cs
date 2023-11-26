using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Item;
using Tzipory.ConfigFiles.Player.Inventory;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.InventorySystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.SerializeData.ItemSerializeData
{
    [System.Serializable]
    public class ItemContainerSerializeData : ISerializeData , ISlotItem , IInitialization<ItemConfig,int>
    {
        public bool IsInitialization { get; private set; }
        
        [SerializeField] private int _itemId;
        [SerializeField] private int _itemStack;
        
        private ItemConfig _itemConfig;
        
        public void Init(ItemConfig itemConfig, int amount)
        {
            _itemConfig = itemConfig;
            
            _itemId = itemConfig.ObjectId;
            _itemStack = amount;
            
            IsInitialization = true;
        }
        
        public void Init(IConfigFile parameter)
        {
            var config = (ItemContainerConfig)parameter;
            
            _itemConfig = config.ItemConfig;
            
            _itemId = config.ObjectId;
            _itemStack = config.Amount;
            
            IsInitialization = true;
        }

        public void AddItemAmount(int amount)=>
            _itemStack += amount;
        public void RemoveItemAmount(int amount)=>
            _itemStack -= amount;

        public int SerializeTypeId => Constant.DataId.ITEM_DATA_ID;
        public Sprite ItemSlotSprite => _itemConfig.ItemIcon;
        public string ItemSlotName => _itemConfig.ItemName;
        public string ItemSlotDescription => _itemConfig.ItemDescription;
        public ItemSlot ItemSlot => _itemConfig.ItemSlot;
        public int ItemId => _itemId;
        public int ItemAmount => _itemStack;
    }
}