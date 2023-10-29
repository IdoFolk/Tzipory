using System;
using Tools.Enums;
using Tzipory.ConfigFiles.Item;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.SerializeData.ItemSerializeData;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIHandler : BaseUIElement , IInitialization<ShamanDataContainer>
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private CharacterItemSlotUI[] _characterItemSlotUis;
    
    private ShamanDataContainer  _shamanDataContainer;
    
    public bool IsInitialization { get; }

    protected override UIGroupType UIGroup => UIGroupType.MetaUI;

    public void Init(ShamanDataContainer parameter)
    {
        _shamanDataContainer = parameter;
        _characterImage.sprite = parameter.UnitEntityVisualConfig.Sprite;

        foreach (var i in _shamanDataContainer.ShamanSerializeData.ItemIDList)
        {
            var itemConfig = DataManager.DataRequester.GetConfigData<ItemConfig>(i);
            
            var itemSlotUi = Array.Find(_characterItemSlotUis, x => x.ItemSlot == itemConfig.ItemSlot);
            itemSlotUi.Init(itemConfig);
        }
        //UIManager.UpdateVisualUIGroup(UIGroupType.MetaUI); // need to add item refresh
    }
    
    private void OnEnable()
    {
        foreach (var itemSlotUi in _characterItemSlotUis)
        {
            itemSlotUi.OnItemDropSuccess += OnItemDropSuccess;
            itemSlotUi.OnItemDropFail += OnItemDropFail;
        }
    }
    
    private void OnDisable()
    {
        foreach (var itemSlotUi in _characterItemSlotUis)
        {
            itemSlotUi.OnItemDropSuccess -= OnItemDropSuccess;
            itemSlotUi.OnItemDropFail -= OnItemDropFail;
        }
    }
    
    private void OnItemDropFail(string failMessage)
    {
        Debug.LogWarning(failMessage);
    }

    private void OnItemDropSuccess(ItemContainerSerializeData data)
    {
        Debug.Log($"Item {data.ItemId} was dropped");
        _shamanDataContainer.ShamanSerializeData.ItemIDList.Add(data.ItemId);
        //remove item from inventory
    }
}
