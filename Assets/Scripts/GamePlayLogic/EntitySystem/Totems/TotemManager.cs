using System;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

public class TotemManager : IDisposable
{
    [SerializeField] private TotemPlacer _totemPlacerPrefab;
    private TotemPlacer _totemPlacer;
    private TotemConfig _totemConfig;
    private Vector3 _totemPlacePos;

    public event Action TotemPlaced;
    public TotemManager(TotemPlacer totemPlacer)
    {
        _totemPlacer = totemPlacer;
        _totemPlacer.Init();
    }
    
    public void PlaceTotem(int shamanId)
    {
        _totemPlacePos = GameManager.CameraHandler.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        _totemPlacePos.z = 0;
        foreach (var shaman in LevelManager.PartyManager.Party)
        {
            if (shaman.EntityInstanceID == shamanId)
            {
                shaman.GoPlaceTotem(_totemPlacePos, PlaceTotem);
                _totemConfig = shaman.TotemConfig;
            }
        } 
    }

    private void PlaceTotem()
    {
        _totemPlacer.PlaceTotem(_totemPlacePos,_totemConfig);
        TotemPlaced?.Invoke();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
