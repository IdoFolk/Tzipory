using System;
using System.Collections.Generic;
using TMPro;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class StatBlockPanel : MonoBehaviour, IInitialization<Dictionary<int, Stat>>
    {
        [SerializeField] private TextMeshProUGUI _healthBarBaseValue; 
        [SerializeField] private TextMeshProUGUI _healthBarValue; 
        [SerializeField] private Image _healthBarFill; 
        [SerializeField] private StatBlockUI[] _statBlocks;
        [SerializeField] private Color _statBonusAdditionColor;
        [SerializeField] private Color _statBonusReductionColor;

        public bool IsInitialization { get; }

        public void Init(Dictionary<int, Stat> stats)
        {
            if (stats.TryGetValue((int)Constant.StatsId.Health, out var healthStat))
            {
                _healthBarBaseValue.text = MathF.Round(healthStat.BaseValue).ToString();
                _healthBarValue.text = MathF.Round(healthStat.CurrentValue).ToString();
                _healthBarFill.fillAmount = healthStat.CurrentValue / healthStat.BaseValue;
            }
            foreach (var statBlock in _statBlocks)
            {
                if (stats.TryGetValue((int)statBlock.StatId,out var stat))
                {
                    statBlock.Init(stat.Name,stat.CurrentValue, _statBonusAdditionColor, _statBonusReductionColor);
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
    }
}