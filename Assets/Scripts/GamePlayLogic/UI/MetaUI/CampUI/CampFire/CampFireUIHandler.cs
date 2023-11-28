using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GameplayLogic.UI.MetaUI.InventoryUI;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.UISystem;
using UnityEngine;

public class CampFireUIHandler : BaseUIElement
{
    [SerializeField] private InventoryUIHandler _inventoryUIHandler;
    [SerializeField] private CharacterUIHandler _characterUIHandler;
    [SerializeField] private CharacterStatsUIHandler _characterStatsUIHandler;

    private void Start()
    {
       // var inventoryData = GameManager.PlayerManager.PlayerSerializeData.InventorySerializeData;
        
        //_inventoryUIHandler.Init(inventoryData);

        //_inventoryUIHandler.OnItemDropToInventory += inventoryData.AddItemData;
    }


    public override void Show()
    {
        base.Show();
        _inventoryUIHandler.Show();
        _characterUIHandler.Show();
        _characterStatsUIHandler.Show();
    }

    public override void Hide()
    {
        //_inventoryUIHandler.OnItemDropToInventory -= GameManager.PlayerManager.PlayerSerializeData.InventorySerializeData.AddItemData;
    }


    public void SetNewShamanData(ShamanDataContainer shamanDataContainer)
    {
        _characterUIHandler.Init(shamanDataContainer);
        _characterStatsUIHandler.Init(shamanDataContainer.ShamanSerializeData);
    }
}
