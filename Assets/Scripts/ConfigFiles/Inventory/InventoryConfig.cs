using Tzipory.ConfigFiles.Item;
using UnityEngine;

namespace Tzipory.ConfigFiles.Inventory
{
    [System.Serializable]
    public class InventoryConfig : IConfigFile
    {
        [SerializeField] private ItemContainerConfig[] _items;
        public int ObjectId { get; }
        public int ConfigTypeId { get; }
    }

    [System.Serializable]
    public class ItemContainerConfig
    {
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private int _amount;

        public ItemConfig ItemConfig => _itemConfig;

        public int Amount => _amount;
    }
}