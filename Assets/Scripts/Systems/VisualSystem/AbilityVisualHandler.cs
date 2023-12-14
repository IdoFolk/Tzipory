using Tzipory.ConfigFiles.Visual;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.VisualSystem
{
    public class AbilityVisualHandler : MonoBehaviour , IInitialization<AnimationConfig>
    {
        private PlayableDirector _currentPlayableDirector;

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
            if (_currentPlayableDirector is not null)
                Destroy(_currentPlayableDirector);
            
            _currentPlayableDirector = Instantiate(_animationConfig.EntryTimeLine, transform);
            _currentPlayableDirector.Play();

            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.EntryTime, "Ability animation Entry Time",SetToLoopStat);
        }

        private void SetToLoopStat()
        {
            Destroy(_currentPlayableDirector.gameObject);
            
            _currentPlayableDirector = Instantiate(_animationConfig.LoopTimeLine, transform);
            _currentPlayableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.LoopTime, "Ability animation Loop Time",SetToExitStat);
        }

        private void SetToExitStat()
        {
            Destroy(_currentPlayableDirector);
            
            _currentPlayableDirector = Instantiate(_animationConfig.LoopTimeLine, transform);
            _currentPlayableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.ExitTime, "Ability animation Exit Time",Stop);
        }

        public void Stop()
        {
            _currentActiveTimer.StopTimer();
        
            if (_currentPlayableDirector is not null)
                Destroy(_currentPlayableDirector);
        }

        private void OnValidate()
        {
        }
    }
}
