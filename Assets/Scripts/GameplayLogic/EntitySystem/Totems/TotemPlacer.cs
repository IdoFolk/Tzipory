using System.Collections;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Unity.Mathematics;
using UnityEngine;

public class TotemPlacer : MonoBehaviour
{
    private List<Totem> _placedTotems;

    public void Init()
    {
        _placedTotems = new List<Totem>();
    }
    public void PlaceTotem(Totem totem)
    {
        var mouseWorldPos = GameManager.CameraHandler.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        var currentTotem = Instantiate(totem, mouseWorldPos, quaternion.identity, transform);
        currentTotem.Init();
        //_placedTotems.Add(currentTotem);
    }
}
