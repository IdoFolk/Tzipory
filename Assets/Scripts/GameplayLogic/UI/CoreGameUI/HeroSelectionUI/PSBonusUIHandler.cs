using System;
using System.Collections.Generic;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

public class PSBonusUIHandler : MonoBehaviour
{
    [SerializeField] private PSBonusUI[] _bonusBlocks;
    public void Show(Dictionary<int,Stat> stats)
    {
        foreach (var bonusBlock in _bonusBlocks)
        {
            if (stats.TryGetValue((int)bonusBlock.StatBonusType,out var stat))
            {
                var bonusValue = MathF.Round((stat.CurrentValue / stat.BaseValue - 1) * 100);
                if (bonusValue <= 0) continue;
                bonusBlock.Show(bonusValue);
            }
        }
    }

    public void Hide()
    {
        foreach (var bonusBlock in _bonusBlocks)
        {
            bonusBlock.Hide();
        }
    }
}
