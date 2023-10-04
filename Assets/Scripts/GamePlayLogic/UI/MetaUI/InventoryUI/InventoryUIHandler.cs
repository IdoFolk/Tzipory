using System.Collections.Generic;
using SerializeData.InventorySerializeData;
using Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;

public class InventoryUIHandler : BaseUIElement , IInitialization<InventorySerializeData>
{
    [SerializeField] private ItemSlotUI _itemSlotUI;
    [SerializeField] private Transform _holder;
    
    private List<ItemSlotUI> _itemSlotUis;
    
    public bool IsInitialization { get; private set; }
    public void Init(InventorySerializeData parameter)
    {
        _itemSlotUis = new List<ItemSlotUI>();
        
        foreach (var itemContainerSerializeData in parameter.ItemData)
        {
            ItemSlotUI  itemSlotUI = Instantiate(_itemSlotUI, _holder);
            itemSlotUI.Init(itemContainerSerializeData);
            _itemSlotUis.Add(itemSlotUI);
        }
        
        IsInitialization = true;
    }
}
