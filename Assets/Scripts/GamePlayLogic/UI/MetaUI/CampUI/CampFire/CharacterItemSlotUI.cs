using System;
using Tzipory.ConfigFiles.Item;
using Tzipory.SerializeData.ItemSerializeData;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterItemSlotUI : BaseInteractiveUIElement
{
    public event Action<ItemContainerSerializeData> OnItemDropSuccess; 
    public event Action<string> OnItemDropFail;
   
    [SerializeField] private ItemSlot _itemSlot;
   
    [SerializeField] private Image _itemSprite;

    public ItemSlot ItemSlot => _itemSlot;

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
      
        if (eventData.pointerDrag.TryGetComponent<ItemSlotUI>(out var itemSlotUI))
        {
            if (itemSlotUI.Item is ItemContainerSerializeData serializeData)
            {
                if (serializeData.ItemSlot == _itemSlot)
                {
                    _itemSprite.sprite = serializeData.ItemSlotSprite;
                    OnItemDropSuccess?.Invoke(serializeData);
                    return;
                }
            
                OnItemDropFail?.Invoke($"Unable to set item slot of type {serializeData.ItemSlot} in {_itemSlot} type");
            }
         
            OnItemDropFail?.Invoke("Cant set non item in this slot");
        }
      
        OnItemDropFail?.Invoke(null);
    }

    public void Init(ItemConfig itemConfig)
    {
        _itemSprite.sprite = itemConfig.ItemIcon;
    }
}