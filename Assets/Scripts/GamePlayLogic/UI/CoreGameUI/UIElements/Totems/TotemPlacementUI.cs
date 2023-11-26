using System.Collections.Generic;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

public class TotemPlacementUI : MonoBehaviour
{
    [SerializeField] private Shadow _shadowPrefab;
    private Dictionary<int,Shadow> _totemShadows;

    public void Init()
    {
        _totemShadows = new Dictionary<int, Shadow>();
        foreach (var shaman in LevelManager.PartyManager.Party)
        {
            if (shaman.TotemConfig is null) return;
            var currentTotem = Instantiate(_shadowPrefab, transform);
            _totemShadows.Add(shaman.EntityInstanceID,currentTotem);
            currentTotem.SetShadowTotem(shaman.TotemConfig.TotemSprite,shaman.TotemConfig.Range);
        }
    }

    public void PlaceShadowTotem(int id,Vector3 pos)
    {
        if (!_totemShadows.TryGetValue(id, out var shadow)) return;
        shadow.transform.position = pos;
        shadow.PlaceShadowTotem();
    }
    public void HideShadowTotem(int id)
    {
        if (!_totemShadows.TryGetValue(id, out var shadow)) return;
        shadow.ClearShadowTotem();
    }
}
