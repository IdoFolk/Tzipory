using Systems.UISystem;
using Tzipory.SerializeData;
using UnityEngine;

public class CampFireUIHandler : BaseUIElement
{
    [SerializeField] private InventoryUIHandler _inventoryUIHandler;
    [SerializeField] private CharacterUIHandler _characterUIHandler;
    [SerializeField] private CharacterStatsUIHandler _characterStatsUIHandler;

    public void SetNewShamanData(ShamanDataContainer shamanDataContainer)
    {
        _characterUIHandler.Init(shamanDataContainer);
        _characterStatsUIHandler.Init(shamanDataContainer.ShamanSerializeData);
    }
}
