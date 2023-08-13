using Helpers.Consts;
using UnityEngine;

[System.Serializable]
public class ProximityIndicatorHandler
{
    public static System.Action TEMP_CallAll_TAB;

    [SerializeField] private Transform _scaler; 
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ProximityConfig _proximityConfig;
    [SerializeField] Tzipory.Helpers.ClickHelper clickHelper;
    
    private float _range;

    private bool _isToggleOn;
    private bool _isLock;
    
    public void Init(float range)
    {
        _range = range;
        _isLock = false;
        _isToggleOn = false;
        //TEMP AF!!!!
        _scaler.localScale = new Vector3(_range , _range, 1);
        //TEMP AF!!!!
        _spriteRenderer.color = _proximityConfig.Color;

        foreach (var condition in _proximityConfig.IndicatorConditions)
        {
            switch (condition)
            {
                case IndicatorCondition.AnyShamanSelected:
                    //Subscribe to OnAnyShamanSelected
                    MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected += StrongSetTrue;
                    MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected += StrongSetFalse;
                    break;
                case IndicatorCondition.HoverSelf:
                    //_spriteRenderer.
                    clickHelper.OnEnterHover += WeakSetTrue;
                    clickHelper.OnExitHover += WeakSetFalse;
                    break;
                case IndicatorCondition.AllCall:
                    //Subscribe to AllCall
                    TEMP_CallAll_TAB += ToggleActive;
                    break;
            }
        }
    }
    /// <summary>
    /// MUST CALL THIS TO UNSUB FROM EVENTS!
    /// </summary>
    public void Disable()
    {
        foreach (var condition in _proximityConfig.IndicatorConditions)
        {
            switch (condition)
            {
                case IndicatorCondition.AnyShamanSelected:
                    //Subscribe to OnAnyShamanSelected
                    MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected -= StrongSetTrue;
                    MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected -= StrongSetFalse;
                    break;
                case IndicatorCondition.HoverSelf:
                    clickHelper.OnEnterHover -= WeakSetTrue;
                    clickHelper.OnExitHover -= WeakSetFalse;
                    //
                    break;
                case IndicatorCondition.AllCall:
                    //Subscribe to AllCall
                    TEMP_CallAll_TAB -= ToggleActive;
                    break;
            }
        }
    }

    void WeakSetFalse()
    {
        WeakSetToActive(false);
    }
    void WeakSetTrue()
    {
        WeakSetToActive(true);
    }
     void StrongSetFalse()
    {
        SetToActiveAndLock(false, false);

    }
    void StrongSetTrue()
    {
        SetToActiveAndLock(true, true);
    }



    void WeakSetToActive(bool doActive)
    {
        //Some logic and stuff
        if (_isLock)
            return;
        _spriteRenderer.enabled = doActive;
    }
    void ToggleActive() //also weak
    {
        //Some logic and stuff
        if (_isLock)
            return;

        _isToggleOn = !_isToggleOn;
        _spriteRenderer.enabled = _isToggleOn;
    }

    void SetToActiveAndLock(bool doActive, bool doLock) //strong!
    {
        //Some logic and stuff
        _isLock = doLock;
        _spriteRenderer.enabled = doActive;

        if (!doActive)
            ResetColor(); //Just to make sure we reset the color 
    }

    //not sure if temp or not (either way, it should change the config, just the current color (temporarly)
    public void ChangeColor(Color col)
    {
        _spriteRenderer.color = col;
    }
    //Resets color back to the color data in config
    public void ResetColor()
    {
        _spriteRenderer.color = _proximityConfig.Color;
    }

}
