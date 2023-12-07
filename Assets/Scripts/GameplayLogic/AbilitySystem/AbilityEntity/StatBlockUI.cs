using System;
using TMPro;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using UnityEngine;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statId;
    [SerializeField] private TextMeshProUGUI _statText;
    [SerializeField] private TextMeshProUGUI _statValue;
    
    private Color _statBonusAdditionColor;
    private Color _statBonusReductionColor;

    private string _name;
    private float _baseValue;
    public Constant.StatsId StatId => _statId;
    
    public void Init(string statName, float currentValue, Color addColor, Color reduceColor)
    {
        _name = statName;
        _baseValue = MathF.Round(currentValue);
        _statBonusAdditionColor = addColor;
        _statBonusReductionColor = reduceColor;
        SetStatText(_baseValue,0);
    }
    public void UpdateUI(float bonusValue)
    {
        if(!HeroSelectionUI.Instance.IsActive) return;
        SetStatText(_baseValue,bonusValue);
    }

    private void SetStatText(float baseValue, float bonusValue)
    {
        string statName = _name;
        string modifier = "";
        switch (_statId)
        {
            case Constant.StatsId.AttackDamage:
                statName = "Damage";
                break;
            case Constant.StatsId.AttackRate:
                statName = "Attack Speed";
                break;
            case Constant.StatsId.AttackRange:
                statName = "Range";
                break;
            case Constant.StatsId.MovementSpeed:
                statName = "Move Speed";
                break;
            case Constant.StatsId.CritDamage:
                statName = "Crit Damage";
                break;
            case Constant.StatsId.CritChance:
                statName = "Crit Chance";
                modifier = "%";
                break;
        }

        _statText.text = statName;
        string modifierText = $"{modifier}";
        string baseValueText = $"{baseValue}";
        string bonusValueText;
        switch (bonusValue)
        {
            case > 0:
                bonusValueText = ColorLogHelper.SetColorToString($" (+{bonusValue})", _statBonusAdditionColor);;
                _statValue.text =  baseValueText + modifierText + bonusValueText;
                break;
            case < 0:
                bonusValueText = ColorLogHelper.SetColorToString($" (-{-bonusValue})", _statBonusReductionColor);;
                _statValue.text = baseValueText + modifierText + bonusValueText;
                break;
            case 0:
                _statValue.text = baseValueText + modifierText;
                break;
        }
    }
}
