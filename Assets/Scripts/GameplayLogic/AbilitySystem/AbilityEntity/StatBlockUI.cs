using TMPro;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Helpers.Consts;
using UnityEngine;

public class StatBlockUI : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statId;
    [SerializeField] private TextMeshProUGUI _statText;

    private string _name;
    private float _baseValue;
    public Constant.StatsId StatId => _statId;
    
    public void Init(string statName, float currentValue)
    {
        _name = statName;
        _baseValue = currentValue;
        SetStatText(_baseValue,0);
    }
    public void UpdateUI(float currentValue)
    {
        if(!HeroSelectionUI.Instance.IsActive) return;
        SetStatText(_baseValue,currentValue);
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

        switch (bonusValue)
        {
            case > 0:
                _statText.text = $"{statName}: {baseValue} (+{bonusValue}{modifier})";
                break;
            case < 0:
                _statText.text = $"{statName}: {baseValue} (-{-bonusValue}{modifier})";
                break;
            case 0:
                _statText.text = $"{statName}: {baseValue}{modifier}";
                break;
        }
    }
}
