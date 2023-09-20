using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.Item
{
    [CreateAssetMenu(fileName = "New item config", menuName = "ScriptableObjects/Inventory/New item config", order = 0)]
    public class ItemConfig : ScriptableObject , IConfigFile
    {
        [SerializeField,TabGroup("Item data")] private string _itemName = "New item";
        [SerializeField,TextArea(4,4),TabGroup("Item data")] private string _itemDescription;
        [SerializeField,TabGroup("Item data")] private ItemSlot _itemSlot;
        [SerializeField,TabGroup("Item data")] private bool _isStackable = true;
        [Space]
        [SerializeField,TabGroup("Item data")] private StatEffectConfig _statEffectConfig;
        [SerializeField,TabGroup("Item Visual")] private Sprite _itemIcon;
        
        private int _objectId = -1;

        private void Awake()
        {
            if (_objectId == -1)
                _objectId = InstanceIDGenerator.GetInstanceID();
        }

        public string ItemDescription => _itemDescription;
        public StatEffectConfig StatEffectConfig => _statEffectConfig;
        public bool IsStackable => _isStackable;
        public string ItemName => _itemName;
        public Sprite ItemIcon => _itemIcon;
        public int ObjectId => _objectId;
        public int ConfigTypeId => Constant.DataId.ITEM_DATA_ID;
    }
    
    public enum ItemSlot
    {
        Head,
        Neck,
        Chest,
        Legs,
        Hands
    }
}