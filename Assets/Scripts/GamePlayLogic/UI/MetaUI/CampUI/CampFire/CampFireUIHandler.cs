using Tzipory.GameplayLogic.Managers.MainGameManagers;
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
        _inventoryUIHandler.Init(GameManager.PlayerManager.PlayerSerializeData.InventorySerializeData);
    }

    public override void Show()
    {
        base.Show();
        _inventoryUIHandler.Show();
        _characterUIHandler.Show();
        _characterStatsUIHandler.Show();
    }


    public void SetNewShamanData(ShamanDataContainer shamanDataContainer)
    {
        _characterUIHandler.Init(shamanDataContainer);
        _characterStatsUIHandler.Init(shamanDataContainer.ShamanSerializeData);
    }
}
