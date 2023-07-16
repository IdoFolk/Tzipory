using System;
using SerializeData.VisualSystemSerializeData;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.PoolSystem;
using Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO;
using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence.EffectType
{
    public class ColorEffectAction : BaseEffectAction , IPoolable<ColorEffectAction>
    {
        private Color _color;
        private Color _originalColor;
        private float _alpha;
        private float _duration;

        protected override float Duration => _duration;

        public override void Init(EffectActionContainerData actionContainerData, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerData, visualComponent);
            
            var config = GetConfig<ColorEffectActionSO>(actionContainerData.EffectActionSo);
            
            _color = config.Color;
            _alpha = config.Alpha;
            _duration = config.Duration;
        }

        protected override void OnStartEffectAction()
        {
            var newColor = new Color(_color.r, _color.g, _color.b, _alpha);
            _originalColor = VisualComponent.SpriteRenderer.color;
            
            VisualComponent.SpriteRenderer.color = newColor;
        }

        protected override void OnProcessEffectAction()
        {
        }

        protected override void OnCompleteEffectAction()
        {
        }

        protected override void OnUndoEffectAction()
        {
            VisualComponent.SpriteRenderer.color = Color.white;
        }

        protected override void OnInterruptEffectAction()
        {
            OnUndoEffectAction();
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