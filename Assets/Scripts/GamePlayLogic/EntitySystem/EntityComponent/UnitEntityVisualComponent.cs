using System;
using Tzipory.ConfigFiles.Visual;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class UnitEntityVisualComponent : MonoBehaviour , IEntityVisualComponent , IInitialization<UnitEntity,EffectSequenceConfig[]>
{
    [SerializeField] private SpriteRenderer _mainSprite;
    
    [SerializeField] private Transform _visualQueueEffectPosition;
    [SerializeField] private PlayableDirector _playableDirector;
    
    private ITimer _currentActiveTimer;
    private AnimationConfig _animationConfig;
    
    public BaseGameEntity GameEntity { get; private set; }
    
    public PopUpTexter PopUpTexter { get; private set; }

    public EffectSequenceHandler EffectSequenceHandler { get; private set; }
    public SpriteRenderer SpriteRenderer => _mainSprite;
    public PlayableDirector ParticleEffectPlayableDirector => _playableDirector;
    public bool IsInitialization { get; private set; }
    
    public void Init(BaseGameEntity parameter)
    {
        GameEntity = parameter;
    }
    
    public void Init(UnitEntity parameter,EffectSequenceConfig[] effectSequence)
    {
        Init(parameter);
        
        PopUpTexter = new PopUpTexter(_visualQueueEffectPosition);
        
        EffectSequenceHandler = new EffectSequenceHandler(this,effectSequence);
        
        IsInitialization = true;
    }
    
    public void UpdateComponent()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        EffectSequenceHandler.UpdateEffectHandler();
    }

    public void StartAnimationEffect(AnimationConfig config)
    {
        _animationConfig = config;
        if (_animationConfig.HaveEnterAndExit)
            SetEntryAnimation();
        else
            SetToLoopAnimation();
    }

    private void SetEntryAnimation()
    {
        _playableDirector.playableAsset = _animationConfig.EntryTimeLine;
        _playableDirector.Play();

        _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.EntryTime, "Ability animation Entry Time",SetToLoopAnimation);
    }
    
    private void SetToLoopAnimation()
    {
        _playableDirector.playableAsset = _animationConfig.LoopTimeLine;
        _playableDirector.Play();
        
        if (_animationConfig.HaveEnterAndExit)  
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.LoopTime, "Ability animation Loop Time",SetToExitAnimation);
    }

    private void SetToExitAnimation()
    {
        _playableDirector.playableAsset = _animationConfig.ExitTimeLine;
        _playableDirector.Play();
        
        _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.ExitTime, "Ability animation Exit Time",StopAnimation);
    }
    
    public void StopAnimation()
    {
        _currentActiveTimer.StopTimer();
        
        if (_playableDirector is not null)
            _playableDirector.Stop();
    }

    public void ResetVisual()
    {
        throw new NotImplementedException();
    }
}
}

