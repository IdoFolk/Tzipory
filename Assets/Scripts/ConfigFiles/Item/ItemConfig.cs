﻿using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.Item
{
    public class ItemConfig : ScriptableObject , IConfigFile
    {
        [SerializeField] public string _itemName = "New item";
        [SerializeField,ReadOnly] public int _objectId = -1;
        [SerializeField,TextArea(4,4),TabGroup("Item data")] private string _itemDescription;
        [SerializeField,PreviewField] private Sprite _itemIcon;
        [SerializeField] private ItemSlot _itemSlot;
        [SerializeField] private bool _isStackable = true;
        [Space]
        [SerializeField] private StatEffectConfig[] _statEffectConfigs;
        
        public string ItemDescription => _itemDescription;
        public StatEffectConfig[] StatEffectConfigs => _statEffectConfigs;
        public bool IsStackable => _isStackable;
        public string ItemName => _itemName;

        public ItemSlot ItemSlot => _itemSlot;

        public Sprite ItemIcon => _itemIcon;
        public int ObjectId => _objectId;
        public int ConfigTypeId => Constant.DataId.ITEM_DATA_ID;
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