using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.VisualSystem
{
    public class AbilityVisualHandler : MonoBehaviour , IInitialization<AbilityVisualConfig>
    {
        [SerializeField] private PlayableDirector _playableDirector;

        private AbilityVisualConfig _abilityVisualConfig;
    
        public bool IsInitialization { get; private set; }

        private ITimer _currentActiveTimer;
    
        public void Init(AbilityVisualConfig parameter)
        {
            _abilityVisualConfig = parameter;
            IsInitialization = true;
        }
    
        public void Play()
        {
            _playableDirector.playableAsset = _abilityVisualConfig.EntryTimeLine;
            _playableDirector.Play();

            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_abilityVisualConfig.EntryTime, "Ability animation Entry Time",SetToLoopStat);
        }

        private void SetToLoopStat()
        {
            _playableDirector.playableAsset = _abilityVisualConfig.LoopTimeLine;
            _playableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_abilityVisualConfig.LoopTime, "Ability animation Loop Time",SetToExitStat);
        }

        private void SetToExitStat()
        {
            _playableDirector.playableAsset = _abilityVisualConfig.ExitTimeLine;
            _playableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_abilityVisualConfig.ExitTime, "Ability animation Exit Time",Stop);
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
