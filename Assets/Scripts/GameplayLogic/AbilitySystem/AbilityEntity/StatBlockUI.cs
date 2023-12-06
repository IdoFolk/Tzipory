using TMPro;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using UnityEngine;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statId;
    [SerializeField] private TextMeshProUGUI _statText;
    
    private Color _statBonusAdditionColor;
    private Color _statBonusReductionColor;

    private string _name;
    private float _baseValue;
    public Constant.StatsId StatId => _statId;
    
    public void Init(string statName, float currentValue, Color addColor, Color reduceColor)
    {
        _name = statName;
        _baseValue = currentValue;
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
                modifier = "Sec";
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

        string statNameText = $"{statName}{modifier}: ";
        string modifierText = $"{modifier}";
        string baseValueText = $"{baseValue} ";
        string bonusValueText;
        switch (bonusValue)
        {
            case > 0:
                bonusValueText = ColorLogHelper.SetColorToString($"(+{bonusValue}{modifier})", _statBonusAdditionColor);;
                _statText.text = statNameText + baseValueText + bonusValueText;
                break;
            case < 0:
                bonusValueText = ColorLogHelper.SetColorToString($"(-{-bonusValue}{modifier})", _statBonusReductionColor);;
                _statText.text = statNameText + baseValueText + bonusValueText;
                break;
            case 0:
                _statText.text = statNameText + baseValueText + modifierText;
                break;
        }
    }
}
