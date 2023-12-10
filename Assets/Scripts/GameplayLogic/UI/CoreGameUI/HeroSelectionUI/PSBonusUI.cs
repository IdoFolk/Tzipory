using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
using Tzipory.Helpers.Consts;
using UnityEngine;
using UnityEngine.UI;

public class PSBonusUI : MonoBehaviour
{
    [SerializeField] private Constant.StatsId _statBonusType;
    [SerializeField] private Sprite _psIconSprite;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private Image _splash;

    public Constant.StatsId StatBonusType => _statBonusType;

    public void Show(float bonusValue)
    {
        gameObject.SetActive(true);
        _splash.sprite = _psIconSprite;
        string bonusText = $"+{bonusValue}%";
        _bonusText.text = bonusText;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
