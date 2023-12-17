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
        [SerializeField] private Transform _animationVisualTransform;
        
        [SerializeField] private Transform _visualQueueEffectPosition;
        [SerializeField] private SpriteRenderer _silhouette;
        
    
        private PlayableDirector _currentPlayableDirector;
        private ITimer _currentActiveTimer;
        private AnimationConfig _animationConfig;
    
        private Vector2 _lastPos;

        public BaseGameEntity GameEntity { get; private set; }
    
        public PopUpTexter PopUpTexter { get; private set; }

        public VisualComponentConfig VisualComponentConfig { get; private set; }
        public EffectSequenceHandler EffectSequenceHandler { get; private set; }
        public SpriteRenderer SpriteRenderer => _mainSprite;
        public IDisposable UIIndicator { get; private set; }
        public PlayableDirector ParticleEffectPlayableDirector => _currentPlayableDirector;
        public bool IsInitialization { get; private set; }

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }
    
        public void Init(BaseGameEntity parameter,VisualComponentConfig config)
        {
            Init(parameter);
            VisualComponentConfig = config;
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
            if (_currentPlayableDirector is not null)
                Destroy(_currentPlayableDirector.gameObject);
            
            _currentPlayableDirector = Instantiate(_animationConfig.EntryTimeLine, _animationVisualTransform);
            _currentPlayableDirector.Play();

            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.EntryTime, "Animation Entry Time",SetToLoopAnimation);
        }
    
        private void SetToLoopAnimation()
        {
            Destroy(_currentPlayableDirector.gameObject);
            
            _currentPlayableDirector = Instantiate(_animationConfig.LoopTimeLine, _animationVisualTransform);
            _currentPlayableDirector.Play();

        
            if (_animationConfig.HaveEnterAndExit)  
                _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.LoopTime, "Animation Loop Time",SetToExitAnimation);
        }

        private void SetToExitAnimation()
        {
            Destroy(_currentPlayableDirector.gameObject);
            
            _currentPlayableDirector = Instantiate(_animationConfig.ExitTimeLine, _animationVisualTransform);
            _currentPlayableDirector.Play();
        
            _currentActiveTimer = GAME_TIME.TimerHandler.StartNewTimer(_animationConfig.ExitTime, "Animation Exit Time",StopAnimation);
        }

        private void StopAnimation()
        {
            _currentActiveTimer.StopTimer();

            if (_currentPlayableDirector is not null)
                Destroy(_currentPlayableDirector.gameObject);

            _currentPlayableDirector = null;
        }

        private void OnValidate()
        {
            _mainSprite  ??= GetComponent<SpriteRenderer>();
            _visualQueueEffectPosition ??= transform.Find("VisualQueueEffectPosition");
        }

        public void ResetVisual()
        {
            throw new NotImplementedException();
        }
    }
}