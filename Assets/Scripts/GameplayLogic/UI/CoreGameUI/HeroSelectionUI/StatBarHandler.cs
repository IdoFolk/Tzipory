using System;
using TMPro;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using UnityEngine.UI;

public class StatBarHandler : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statType;
    [SerializeField] private TextMeshProUGUI _statBarBaseValue;
    [SerializeField] private TextMeshProUGUI _statBarValue;
    [SerializeField] private Image _statBarFill;

    private float _baseStatValue;
    private Stat _stat;

    public Constant.StatsId StatType => _statType;

    public void Init(Stat stat)
    {
        _statBarBaseValue.text = MathF.Round(stat.BaseValue).ToString();
        _statBarValue.text = MathF.Round(stat.CurrentValue).ToString();
        _statBarFill.fillAmount = stat.CurrentValue / stat.BaseValue;
        _baseStatValue = stat.BaseValue;
        _stat = stat;
        stat.OnValueChanged += UpdateStatBar;
    }

    public void UpdateStatBar(StatChangeData statChangeData)
    {
        _statBarValue.text = MathF.Round(statChangeData.NewValue).ToString();
        _statBarFill.fillAmount = statChangeData.NewValue / _baseStatValue;
    }

    public void Hide()
    {
        _stat.OnValueChanged -= UpdateStatBar;
    }
}