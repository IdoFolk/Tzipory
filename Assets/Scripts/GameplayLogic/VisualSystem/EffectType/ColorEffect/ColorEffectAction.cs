using System;
using Tzipory.Systems.StatusSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.StatusSystem.EffectActionTypeSO;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType.ColorEffect
{
    public class ColorEffectAction : BaseEffectAction , IPoolable<ColorEffectAction>
    {
        private UnityEngine.Color _color;
        private UnityEngine.Color _originalColor;
        private float _alpha;
        private float _duration;

        public override float Duration => _duration;

        public override void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerConfig, visualComponent);
            
            var config = GetConfig<ColorEffectActionConfig>(actionContainerConfig.EffectActionConfig);
            
            _color = config.Color;
            _alpha = config.Alpha;
            _duration = config.Duration;
        }

        public override void StartEffectAction()
        {
            var newColor = new UnityEngine.Color(_color.r, _color.g, _color.b, _alpha);
            _originalColor = VisualComponent.SpriteRenderer.color;
            
            VisualComponent.SpriteRenderer.color = newColor;
        }

        public override void ProcessEffectAction()
        {
        }

        public override void CompleteEffectAction()
        {
        }

        public override void UndoEffectAction()
        {
            VisualComponent.SpriteRenderer.color = UnityEngine.Color.white;
        }

        public override void InterruptEffectAction()
        {
            UndoEffectAction();
        }

        #region PoolObject

        public event Action<ColorEffectAction> OnDispose;
        public void Dispose()=>
            OnDispose?.Invoke(this);

        public void Free()
        {
            
        }

        #endregion
    }
}