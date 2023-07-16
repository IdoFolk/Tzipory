using System;
using SerializeData.VisualSystemSerializeData;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.PoolSystem;
using Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO;
using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence.EffectType
{
    public class TransformEffectAction : BaseEffectAction , IPoolable<TransformEffectAction>
    {
        private TransformEffectActionSO _transformEffectActionSO;

        private Vector3 _originalPosition;
        private Vector3 _originalScale;
        private Vector3 _originalRotation;

        protected override float Duration
        {
            get
            {
                float moveDuration = 0;
                float scaleDuration = 0;
                float rotationDuration = 0;
                
                if (_transformEffectActionSO.HaveMovement)
                    moveDuration = _transformEffectActionSO.Movement.TimeToTransition;
                if (_transformEffectActionSO.HaveScale)
                    scaleDuration = _transformEffectActionSO.Scale.TimeToTransition;
                if (_transformEffectActionSO.HaveRotation)
                    rotationDuration  = _transformEffectActionSO.Rotation.TimeToTransition;
                
                
                return Mathf.Max(moveDuration,scaleDuration,rotationDuration);
            }
        }

        public override void Init(EffectActionContainerData actionContainerData, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerData, visualComponent);
            
            var config = GetConfig<TransformEffectActionSO>(actionContainerData.EffectActionSo);
            
            _transformEffectActionSO  = config;
        }

        protected override void OnStartEffectAction()
        {
            _originalPosition = VisualComponent.EntityTransform.position;
            _originalScale = VisualComponent.EntityTransform.localScale;
            _originalRotation = VisualComponent.EntityTransform.eulerAngles;
            
            VisualComponent.EntityTransform.Transition(_transformEffectActionSO);
        }

        protected override void OnProcessEffectAction()
        {
        }

        protected override void OnCompleteEffectAction()
        {
            VisualComponent.EntityTransform.Move(_originalPosition,_transformEffectActionSO);
            VisualComponent.EntityTransform.Scale(_originalScale,_transformEffectActionSO);
            VisualComponent.EntityTransform.Rotate(_originalRotation,_transformEffectActionSO);
        }

        protected override void OnUndoEffectAction()
        {
            OnCompleteEffectAction();
        }

        protected override void OnInterruptEffectAction()
        {
            OnCompleteEffectAction();
        }

        #region PoolObject

        public event Action<TransformEffectAction> OnDispose;
        public void Dispose() => OnDispose?.Invoke(this);

        public void Free()
        {
            _transformEffectActionSO = null;
        }

        #endregion
    }
    
    [Serializable]
    public class Transition3D 
    {
        [SerializeField] private float _timeToTransition = 1.0f;

        [SerializeField] private AnimationCurve _animationCurveX;
            
        [SerializeField] private AnimationCurve _animationCurveY;
            
        [SerializeField] private AnimationCurve _animationCurveZ;
        
        public float TimeToTransition
        {
            get { return _timeToTransition;}
#if UNITY_EDITOR
            set { _timeToTransition = value; }
        
#endif
        }
        public AnimationCurve AnimationCurveX
        {
            get { return _animationCurveX;}
#if UNITY_EDITOR
            set { _animationCurveX = value; }

#endif
        }
        public AnimationCurve AnimationCurveY
        {
            get { return _animationCurveY;}
#if UNITY_EDITOR
            set { _animationCurveY = value; }

#endif
        }
        public AnimationCurve AnimationCurveZ
        {
            get { return _animationCurveZ;}
#if UNITY_EDITOR
            set { _animationCurveZ = value; }

#endif
        }
    } 

}