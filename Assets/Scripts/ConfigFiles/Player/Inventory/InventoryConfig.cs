using Tzipory.ConfigFiles.Item;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.Player.Inventory
{
    [System.Serializable]
    public class InventoryConfig : IConfigFile
    {
        [SerializeField] private ItemContainerConfig[] _items;
        //TODO: add potions
        public ItemContainerConfig[] Items => _items;

        public int ObjectId { get; }
        public int ConfigTypeId { get; }
    }

    [System.Serializable]
    public class ItemContainerConfig : IConfigFile
    {
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private int _amount;

        public ItemConfig ItemConfig => _itemConfig;

        public int Amount => _amount;
        public int ObjectId => _itemConfig.ObjectId;
        public int ConfigTypeId => Constant.DataId.ITEM_DATA_ID;
    }
}