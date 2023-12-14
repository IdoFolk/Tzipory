using System.Collections.Generic;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class StatBlockPanel : MonoBehaviour, IInitialization<IEnumerable<Stat>>
    {
        [SerializeField] private StatBlockUI[] _statBlocks;
        [SerializeField] private StatBarHandler[] _statBarHandlers;
        [SerializeField] private Color _statBonusAdditionColor;
        [SerializeField] private Color _statBonusReductionColor;

        public bool IsInitialization { get; }

        public void Init(IEnumerable<Stat> stats)
        {
            foreach (var statBlock in _statBlocks)
            {
                foreach (var stat in stats)
                {
                    if ((int)statBlock.StatId == stat.Id)
                        statBlock.Init(stat.Name, stat.CurrentValue, _statBonusAdditionColor, _statBonusReductionColor);
                }
            }

            foreach (var statBar in _statBarHandlers)
            {
                foreach (var stat in stats)
                {
                    if ((int)statBar.StatType == stat.Id)
                        statBar.Init(stat);
                }
            }
        }

        public void UpdateStatBlocks(Stat shamanStat, float newValue)
        {
            foreach (var statBlock in _statBlocks)
            {
                if ((int)statBlock.StatId == shamanStat.Id)
                {
                    statBlock.UpdateUI(newValue);
                }
            }
        }

        public void HideStatBlocks()
        {
            foreach (var statBarHandler in _statBarHandlers)
            {
                statBarHandler.Hide();
            }
        }
    }
}