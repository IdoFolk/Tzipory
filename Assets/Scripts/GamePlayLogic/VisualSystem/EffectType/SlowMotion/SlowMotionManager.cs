using System;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Helpers;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Tools.Sound;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    public class SlowMotionManager : MonoSingleton<SlowMotionManager>
    {
        [SerializeField] private AudioFilters _audioFilters;
        [SerializeField] private PostProcessSlowMotionEffect _postProcessFilters;
        [SerializeField] private WindEffectHandler _windEffectHandler;

        [TabGroup("Time"),SerializeField] private AnimationCurve _startSlowTimeCurve;
        [TabGroup("Time"),SerializeField] private AnimationCurve _endSlowTimeCurve;
        [TabGroup("Time"),SerializeField] private float _slowTimeTransitionTime;
        [TabGroup("Time"),SerializeField] private float _slowTime;
        private float _previousTimeRate;
        
        private void Start()
        {
            _audioFilters.Init(BgMusicManager.Instance.AudioSource,
                BgMusicManager.Instance.AudioReverbFilter, BgMusicManager.Instance.AudioLowPassFilter);
            _postProcessFilters.Init(GameManager.CameraHandler.PostProcessVolume);
            _windEffectHandler.Init(LevelHandler.ParticleSystems);
        }

        public void StartSlowMotionEffects()
        {
            
            _previousTimeRate = GAME_TIME.GetCurrentTimeRate;
            GAME_TIME.SetTimeStep(_slowTime, _slowTimeTransitionTime, _startSlowTimeCurve);
            _audioFilters.StartTransitionEffect();
            _windEffectHandler.StartTransitionEffect();
            _postProcessFilters.StartTransitionEffect();
        }

        public void EndSlowMotionEffects()
        {
            GAME_TIME.SetTimeStep(_previousTimeRate, _slowTimeTransitionTime, _endSlowTimeCurve);
            _audioFilters.EndTransitionEffect();
            _windEffectHandler.EndTransitionEffect();
            _postProcessFilters.EndTransitionEffect();
        }
    }
}