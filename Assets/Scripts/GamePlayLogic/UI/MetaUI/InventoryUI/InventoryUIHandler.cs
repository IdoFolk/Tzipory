using System;
using System.Collections.Generic;
using Tools.Enums;
using Tzipory.SerializeData.Inventory;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUIHandler : BaseInteractiveUIElement , IInitialization<InventorySerializeData>
{
    public event Action<int, int> OnItemDropToInventory; 
    
    [SerializeField] private ItemSlotUI _itemSlotUI;
    [SerializeField] private Transform _holder;
    
    private List<ItemSlotUI> _itemSlotUis;

    protected override UIGroupType UIGroup => UIGroupType.MetaUI;
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

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        var characterItemSlotUI = eventData.pointerDrag.GetComponentInParent<CharacterItemSlotUI>();

        if (characterItemSlotUI is null)
            return;

        if (!characterItemSlotUI.HaveItem)
            return;
        
        OnItemDropToInventory?.Invoke(characterItemSlotUI.StoreItemId,1);
    }

}
