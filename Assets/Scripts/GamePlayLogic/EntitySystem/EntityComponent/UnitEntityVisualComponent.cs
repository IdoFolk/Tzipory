using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.ConfigFiles.Visual;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class UnitEntityVisualComponent : MonoBehaviour , IEntityVisualComponent 
    {
        public event Action<Sprite> OnSetSprite;
    
        public event Action<bool> OnSpriteFlipX;
    
        [SerializeField] private SpriteRenderer _mainSprite;
    
        [SerializeField] private Transform _visualQueueEffectPosition;
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] private SpriteRenderer _silhouette;
        
    
        private ITimer _currentActiveTimer;
        private AnimationConfig _animationConfig;
    
        private Vector2 _lastPos;

        public BaseGameEntity GameEntity { get; private set; }
    
        public PopUpTexter PopUpTexter { get; private set; }

        public VisualComponentConfig VisualComponentConfig { get; private set; }
        public EffectSequenceHandler EffectSequenceHandler { get; private set; }
        public SpriteRenderer SpriteRenderer => _mainSprite;
        public IDisposable UIIndicator { get; private set; }
        public PlayableDirector ParticleEffectPlayableDirector => _playableDirector;
        public bool IsInitialization { get; private set; }

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }
    
        public void Init(BaseGameEntity parameter,VisualComponentConfig config)
        {
            Init(parameter);
        
            config.OnDeath.ID = Constant.EffectSequenceIds.DEATH;
            config.OnAttack.ID = Constant.EffectSequenceIds.ATTACK;
            config.OnCritAttack.ID = Constant.EffectSequenceIds.CRIT_ATTACK;
            config.OnMove.ID = Constant.EffectSequenceIds.MOVE;
            config.OnGetHit.ID = Constant.EffectSequenceIds.GET_HIT;
            config.OnGetCritHit.ID = Constant.EffectSequenceIds.GET_CRIT_HIT;
            
            config.OnDeath.SequenceName = "OnDeath";
            config.OnAttack.SequenceName = "OnAttack";
            config.OnCritAttack.SequenceName = "OnCritAttack";
            config.OnMove.SequenceName = "OnMove";
            config.OnGetHit.SequenceName = "OnGetHit";
            config.OnGetCritHit.SequenceName = "OnGetCritHit";
            
            var effectSequence = new[]
            {
                config.OnDeath,
                config.OnAttack,
                config.OnCritAttack,
                config.OnMove,
                config.OnGetHit,
                config.OnGetCritHit
            };
        
            PopUpTexter = new PopUpTexter(_visualQueueEffectPosition);

            if (config.UIIndicator)
                UIIndicator = UIIndicatorHandler.SetNewIndicator(GameEntity.transform,config.UiIndicatorConfig,null,GameEntity.FocusOnEntity);
        
            EffectSequenceHandler = new EffectSequenceHandler(this,effectSequence);

            if (config.HaveSilhouette)
            {
                _silhouette.gameObject.SetActive(true);
                _silhouette.sprite = config.Sprite;
            }
            else
                _silhouette.gameObject.SetActive(false);
            
            
            SetSprite(config.Sprite);
        
            IsInitialization = true;
        }
    
        public void UpdateComponent()
        {
            EffectSequenceHandler.UpdateEffectHandler();

            var position = (Vector2)transform.position;
            _lastPos = position;
            var deltaV = position - _lastPos;
        
            if (deltaV.sqrMagnitude >= 0.1f)
                SetSpriteFlipX(deltaV.x >= 0);

            // if (VisualComponentConfig.HaveSilhouette)
            // {
            //     if (Physics.Raycast(transform.position, -Vector3.forward, out var hit, 100f))
            //     {
            //         if (hit.)
            //         {
            //             
            //         }
            //     }
            // }
        }
    
        private void SetSprite(Sprite newSprite)
        {
            SpriteRenderer.sprite = newSprite;
            OnSetSprite?.Invoke(newSprite);
        }
    
        public void SetSpriteFlipX(bool doFlip)
        {
            SpriteRenderer.flipX = doFlip;
            OnSpriteFlipX?.Invoke(doFlip);
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

        private void StopAnimation()
        {
            _currentActiveTimer.StopTimer();
        
            if (_playableDirector is not null)
                _playableDirector.Stop();
        }

        private void OnValidate()
        {
            _mainSprite  ??= GetComponent<SpriteRenderer>();
            _visualQueueEffectPosition ??= transform.Find("VisualQueueEffectPosition");
            _playableDirector ??= GetComponent<PlayableDirector>();
        }

        public void ResetVisual()
        {
            throw new NotImplementedException();
        }
    }
}