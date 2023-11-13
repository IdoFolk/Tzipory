using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Unity.Mathematics;
using UnityEngine;

public class TotemsManager : MonoBehaviour
{
    [SerializeField] private TotemPlacer _totemPlacerPrefab;
    private static TotemPlacer _totemPlacer;

    public void init()
    {
        _totemPlacer = Instantiate(_totemPlacerPrefab, transform);
        _totemPlacer.Init();
    }
    
    public static void PlaceTotem(int shamanId)
    {
        _totemPlacer.PlaceTotem(shamanId);
    }
}
