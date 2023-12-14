using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.StatusSystem.EffectActionTypeSO;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.TransitionSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    public class TransformEffectAction : BaseEffectAction , IPoolable<TransformEffectAction>
    {
        private TransformEffectActionConfig _transformEffectActionConfig;

        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private Vector3 _originalRotation;

        public override float Duration
        {
            get
            {
                float moveDuration = 0;
                float scaleDuration = 0;
                float rotationDuration = 0;
                
                if (_transformEffectActionConfig.HaveMovement)
                    moveDuration = _transformEffectActionConfig.Movement.TimeToTransition;
                if (_transformEffectActionConfig.HaveScale)
                    scaleDuration = _transformEffectActionConfig.Scale.TimeToTransition;
                if (_transformEffectActionConfig.HaveRotation)
                    rotationDuration  = _transformEffectActionConfig.Rotation.TimeToTransition;
                
                
                return Mathf.Max(moveDuration,scaleDuration,rotationDuration);
            }
        }

        public override void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerConfig, visualComponent);
            
            var config = GetConfig<TransformEffectActionConfig>(actionContainerConfig.EffectActionConfig);
            
            _transformEffectActionConfig  = config;
        }

        public override void StartEffectAction()
        {
            var transform = VisualComponent.GameEntity.transform;
            
            _originalPosition = transform.position;
            _originalScale = transform.localScale;
            _originalRotation = transform.eulerAngles;
            
            transform.Transition(_transformEffectActionConfig);
        }

        public override void ProcessEffectAction()
        {
        }

        public override void CompleteEffectAction()
        {
            VisualComponent.GameEntity.transform.Move(_originalPosition,_transformEffectActionConfig);
            VisualComponent.GameEntity.transform.Scale(_originalScale,_transformEffectActionConfig);
            VisualComponent.GameEntity.transform.Rotate(_originalRotation,_transformEffectActionConfig);
        }

        public override void UndoEffectAction()
        {
            CompleteEffectAction();
        }

        public override void InterruptEffectAction()
        {
            CompleteEffectAction();
        }

        #region PoolObject

        public event Action<TransformEffectAction> OnDispose;
        public void Dispose() => OnDispose?.Invoke(this);

        public void Free()
        {
            _transformEffectActionConfig = null;
        }

        #endregion
    }
}