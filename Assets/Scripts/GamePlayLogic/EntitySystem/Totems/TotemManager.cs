using System;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.UIElements;
using Tzipory.Helpers;
using UnityEngine;

public class TotemManager : MonoSingleton<TotemManager>
{
    [SerializeField] private TotemPlacer _totemPlacer;
    [SerializeField] private TotemPanelUIManager _totemPanelUIManager;
    private TotemConfig _totemConfig;

    public static event Action TotemPlaced;

    private void Start()
    {
        _totemPlacer.Init();
        _totemPanelUIManager.TotemClicked += SelectTotem;
    }

    
    public void PlaceTotem(Vector3 pos, Shaman connectedShaman)
    {
        _totemPlacer.PlaceTotem(pos, connectedShaman.TotemConfig, connectedShaman);
        TotemPlaced?.Invoke();
    }

    public void SelectTotem(int totemId, int shamanId)
    {
        foreach (var shaman in LevelManager.PartyManager.Party.Where(shaman => shaman.EntityInstanceID == shamanId))
        {
            TotemPanelUIManager.ToggleTotemSelected(true);
            shaman.TempHeroMovement.SelectHero();
        }
    }
    public void Dispose()
    {
        _totemPanelUIManager.TotemClicked -= SelectTotem;
    }
}
