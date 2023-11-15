using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Unity.Mathematics;
using UnityEngine;

public class TotemPlacer : MonoBehaviour
{
    private Totem _totemPrefab;
    private List<Totem> _placedTotems;
    
    public void Init()
    {
        _totemPrefab = Resources.Load<Totem>("Prefabs/Entities/Totems/Totem");
        _placedTotems = new List<Totem>();
    }
    public void PlaceTotem(Vector3 pos, TotemConfig totemConfig, Shaman connectedShaman)
    {
        if (_placedTotems is not null)
        {
            foreach (var totem in _placedTotems.Where(totem => totem.ConnectedShaman.EntityInstanceID == connectedShaman.EntityInstanceID))
            {
                totem.transform.position = pos;
                return;
            } 
        }
        var currentTotem = Instantiate(_totemPrefab, pos, quaternion.identity, transform);
        currentTotem.Init(totemConfig,connectedShaman);
        _placedTotems.Add(currentTotem);
    }
}
