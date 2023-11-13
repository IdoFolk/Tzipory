using System.Collections;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Unity.Mathematics;
using UnityEngine;

public class TotemPlacer : MonoBehaviour
{
    //private List<Totem> _placedTotems;
    private Totem _totemPrefab;
    
    public void Init()
    {
        //_placedTotems = new List<Totem>();
        _totemPrefab = Resources.Load<Totem>("Prefabs/Entities/Totems/Totem");
    }
    public void PlaceTotem(int shamanId)
    {
        var totemPlacePos = GameManager.CameraHandler.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        totemPlacePos.z = 0;
        foreach (var shaman in LevelManager.PartyManager.Party)
        {
            if (shaman.EntityInstanceID == shamanId)
            {
                shaman.GoPlaceTotem(totemPlacePos);
            }
        } 
        
        //var currentTotem = Instantiate(_totemPrefab, totemPlacePos, quaternion.identity, transform);
        //currentTotem.Init();
        //_placedTotems.Add(currentTotem);
    }
}
