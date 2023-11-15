using System;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.Helpers;
using UnityEngine;

public class TotemManager : MonoSingleton<TotemManager>
{
    [SerializeField] private TotemPlacer _totemPlacer;
    private TotemConfig _totemConfig;

    public static event Action TotemPlaced;

    private void Start()
    {
        _totemPlacer.Init();
    }


    public void PlaceTotem(Vector3 pos, TotemConfig totemConfig)
    {
        _totemPlacer.PlaceTotem(pos,totemConfig);
        TotemPlaced?.Invoke();
    }


    public void Dispose()
    {
        // TODO release managed resources here
    }
}
