using System;
using TMPro;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statId;
    [SerializeField] private TextMeshProUGUI _statText;

    private string _name;
    private float _baseValue;

    public Constant.StatsId StatId => _statId;
    
    public void Init(string statName, float baseValue, float currentValue)
    {
        _name = statName;
        _baseValue = baseValue;
        var _delta = currentValue - baseValue;
        SetStatText(baseValue,_delta);
    }
    public void UpdateUI(StatChangeData statChangeData)
    {
        if(!HeroSelectionUI.Instance.IsActive) return;
        var _delta = statChangeData.NewValue -_baseValue;
        SetStatText(_baseValue,_delta);
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
                modifier = "Sc";
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
        
        if (bonusValue == 0) _statText.text = $"{statName}: {baseValue}{modifier}";
        else _statText.text = $"{statName}: {baseValue} + {bonusValue}{modifier}";
    }
}
