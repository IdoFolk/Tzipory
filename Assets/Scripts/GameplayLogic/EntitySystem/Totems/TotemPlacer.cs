using System.Collections;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Unity.Mathematics;
using UnityEngine;

public class TotemPlacer : MonoBehaviour
{
    private Totem _totemPrefab;
    
    public void Init()
    {
        _totemPrefab = Resources.Load<Totem>("Prefabs/Entities/Totems/Totem");
    }
    public void PlaceTotem(Vector3 pos, TotemConfig totemConfig)
    {
        var currentTotem = Instantiate(_totemPrefab, pos, quaternion.identity, transform);
        currentTotem.Init(totemConfig);
    }
}
