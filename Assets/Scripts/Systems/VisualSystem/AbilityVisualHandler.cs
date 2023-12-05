using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.Visual;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.VisualSystem
{
    public class AbilityVisualHandler : MonoBehaviour , IInitialization<AnimationConfig>
    {
        [SerializeField] private PlayableDirector _playableDirector;

        private AnimationConfig _animationConfig;
    
        public bool IsInitialization { get; private set; }

        private ITimer _currentActiveTimer;
    
        public void Init(AnimationConfig parameter)
        {
            _animationConfig = parameter;
            Play();
            IsInitialization = true;
        }
    
        public void Play()
        {
            _playableDirector.playableAsset = _animationConfig.EntryTimeLine;
            _playableDirector.Play();

            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.EntryTime, "Ability animation Entry Time",SetToLoopStat);
        }

        private void SetToLoopStat()
        {
            _playableDirector.playableAsset = _animationConfig.LoopTimeLine;
            _playableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.LoopTime, "Ability animation Loop Time",SetToExitStat);
        }

        private void SetToExitStat()
        {
            _playableDirector.playableAsset = _animationConfig.ExitTimeLine;
            _playableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.ExitTime, "Ability animation Exit Time",Stop);
        }

        public void Stop()
        {
            _currentActiveTimer.StopTimer();
        
            if (_playableDirector is not null)
                _playableDirector.Stop();
        }

        private void OnValidate()
        {
            _playableDirector ??= GetComponentInChildren<PlayableDirector>();
        }
    }
}
