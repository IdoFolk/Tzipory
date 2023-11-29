using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Helpers;
using Tzipory.Tools.Sound;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

public class SlowMotionManager : MonoSingleton<SlowMotionManager>
{
    [SerializeField] private AnimationCurve _startSlowTimeCurve;
    [SerializeField] private AnimationCurve _endSlowTimeCurve;
    [SerializeField] private float _slowTimeTransitionTime;
    [SerializeField] private float _slowTime;
    
    private float _previousTimeRate;

    public void StartSlowMotionEffect()
    {
        _previousTimeRate = GAME_TIME.GetCurrentTimeRate;
        GAME_TIME.SetTimeStep(_slowTime,_slowTimeTransitionTime,_startSlowTimeCurve);
        BgMusicManager.Instance.SetSlowMotionEffect(_slowTimeTransitionTime,_startSlowTimeCurve); 
        GameManager.CameraHandler.SetSlowMotionVisualFX(_slowTimeTransitionTime,_startSlowTimeCurve);
    }

    public void EndSlowMotionEffect()
    {
        GAME_TIME.SetTimeStep(_previousTimeRate,_slowTimeTransitionTime,_endSlowTimeCurve);
        BgMusicManager.Instance.SetDefaultEffect(_slowTimeTransitionTime,_endSlowTimeCurve); 
        GameManager.CameraHandler.EndSlowMotionVisualFX(_slowTimeTransitionTime,_endSlowTimeCurve);
    }
}
