using System.Collections.Generic;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class StatBlockPanel : MonoBehaviour, IInitialization<Dictionary<int, Stat>>
    {
        [SerializeField] private StatBlockUI[] _statBlocks;
        [SerializeField] private Color _statBonusAdditionColor;
        [SerializeField] private Color _statBonusReductionColor;

        public bool IsInitialization { get; }

        public void Init(Dictionary<int, Stat> stats)
        {
            foreach (var statBlock in _statBlocks)
            {
                if (stats.TryGetValue((int)statBlock.StatId,out var stat))
                {
                    statBlock.Init(stat.Name,stat.CurrentValue, _statBonusAdditionColor, _statBonusReductionColor);
                }
            }
        }

        public void UpdateStatBlocks(Stat shamanStat, Stat shadowStat)
        {
            foreach (var statBlock in _statBlocks)
            {
                if ((int)statBlock.StatId == shamanStat.Id)
                {
                    var delta = shadowStat.CurrentValue - shamanStat.CurrentValue;
                    statBlock.UpdateUI(delta);
                }
            }
        }
    }
}