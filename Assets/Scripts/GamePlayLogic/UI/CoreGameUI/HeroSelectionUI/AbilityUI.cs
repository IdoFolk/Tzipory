using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.AbilitySystem;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image _abilitySpriteRenderer;
    [SerializeField] private Image _cooldownSpriteRenderer;
    
    private AbilityConfig _abilityConfig;

    private bool _isActive;
    private Ability _ability;

    public bool IsActive => _isActive;

    private void Start()
    {
        _abilitySpriteRenderer.enabled = false;
        _cooldownSpriteRenderer.enabled = false;
    }

    public void Show(Ability ability)
    {
        _ability = ability;
        _abilitySpriteRenderer.enabled = true;
        _cooldownSpriteRenderer.enabled = true;
        _abilitySpriteRenderer.sprite = ability.Config.AbilityVisualConfig.AbilityIcon;
        _isActive = true;
    }
    private void Update()
    {
        if (!_isActive) return;
        UpdateCooldownFillAmount(_ability);
    }
    private void UpdateCooldownFillAmount(Ability ability)
    {
        float ratio =  ability.CooldownTimeRemaining /ability.Config.Cooldown;
        if (ratio < 0) ratio = 0;
        _cooldownSpriteRenderer.fillAmount = ratio;
    }
    public void Hide()
    {
        _abilitySpriteRenderer.enabled = false;
        _cooldownSpriteRenderer.enabled = false;
        _isActive = false;
    }
}
