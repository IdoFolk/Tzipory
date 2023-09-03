using UnityEngine;
[System.Serializable]
public class RingedProximityIndicatorHandler : ProximityIndicatorHandler
{
    [SerializeField] private SpriteRenderer[] _renderers;
    [SerializeField] private Transform[] _scalers;
    /// <summary>
    /// Inits the ring with several ranges for the ringed proximity ranges 
    /// This will also call base.Init() so no need to call that again
    /// </summary>
    /// <param name="ranges"></param>
    public void InitWithRanges(float[] ranges)
    {

        for (int i = 1; i < _scalers.Length; i++) //the outer ring (0) is set in base.Init(ranges[0])
        {
            _scalers[i].localScale = Vector3.one * ranges[i];
        }

        base.Init(ranges[0]);
    }

    protected override void ToggleActive()
    {
        if (_isLock)
            return;

        _isToggleOn = !_isToggleOn;
        foreach (var rend in _renderers)
        {
            rend.enabled = _isToggleOn;
        }
    }

    protected override void WeakSetToActive(bool doActive)
    {
        if (_isLock)
            return;

        foreach (var rend in _renderers)
        {
            rend.enabled = doActive;
        }
    }

    protected override void SetToActiveAndLock(bool doActive, bool doLock)
    {
        _isLock = doLock;

        foreach (var rend in _renderers)
        {
            rend.enabled = doActive;
        }

        //Deal with colours later please TBD
        //if (!doActive) 
        //    ResetColor(); //Just to make sure we reset the color 
    }
}
