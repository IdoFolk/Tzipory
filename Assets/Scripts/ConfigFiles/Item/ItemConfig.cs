using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.Recipe;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.Item
{
    public class ItemConfig : ScriptableObject , IConfigFile
    {
        [SerializeField] public string _itemName = "New item";
        [ReadOnly] public int _objectId;
        [SerializeField,TextArea(4,4),TabGroup("Item data")] private string _itemDescription;
        [SerializeField,PreviewField] private Sprite _itemIcon;
        [SerializeField] private ItemSlot _itemSlot;
        [SerializeField] private bool _isStackable = true;
        [Space]
        [SerializeField] private StatEffectConfig[] _statEffectConfigs;
        [SerializeField] private RecipeConfig _recipe;
        public string ItemDescription => _itemDescription;
        public StatEffectConfig[] StatEffectConfigs => _statEffectConfigs;
        public bool IsStackable => _isStackable;
        public string ItemName => _itemName;

        public ItemSlot ItemSlot => _itemSlot;

        public Sprite ItemIcon => _itemIcon;
        public int ObjectId => _objectId;
        public int ConfigTypeId => Constant.DataId.ITEM_DATA_ID;

        public RecipeConfig Recipe => _recipe;
    }
    
    public enum ItemSlot
    {
        Necklace,
        Earring,
        Belt,
        Bracelet,
        Ring
    }
}