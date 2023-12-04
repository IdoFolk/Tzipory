using System.Collections.Generic;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class StatBlockPanel : MonoBehaviour, IInitialization<Dictionary<int, Stat>>
    {
        [SerializeField] private StatBlockUI[] _statBlocks;

        public bool IsInitialization { get; }

        public void Init(Dictionary<int, Stat> stats)
        {
            foreach (var statBlock in _statBlocks)
            {
                if (stats.TryGetValue((int)statBlock.StatId,out var stat))
                {
                    statBlock.Init(stat.Name,stat.BaseValue,stat.CurrentValue);
                    stat.OnValueChanged += statBlock.UpdateUI; //add unsubscribe
                }
            }
        }
    }
}