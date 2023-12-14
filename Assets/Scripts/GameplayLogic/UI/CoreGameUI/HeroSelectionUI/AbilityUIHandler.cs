using System.Collections.Generic;
using Tzipory.Systems.AbilitySystem;
using UnityEngine;

public class AbilityUIHandler : MonoBehaviour
{
    [SerializeField] private AbilityUI[] _abilityUIBlocks;
    
    public void Show(Dictionary<string,Ability> abilities)
    {
        foreach (var ability in abilities)
        {
            foreach (var uiBlock in _abilityUIBlocks)
            {
                if (uiBlock.IsActive) return;
                uiBlock.Show(ability.Value);
                break;
            }
        }
    }

    public void Hide()
    {
        foreach (var uiBlock in _abilityUIBlocks)
        {
            if (!uiBlock.IsActive) return;
            uiBlock.Hide();
        }
    }
}
