using System;
using DG.Tweening;
using Tzipory.Systems.StatusSystem.EffectActionTypeSO;
using UnityEngine;

namespace Tzipory.Systems.VisualSystem.TransitionSystem
{
    public static class TransitionManager
    {
        #region PublicFunction

        #region Transitions

        public static Sequence Transition(this Transform transform, TransformEffectActionConfig transitionPackConfig,
            Action onComplete = null)
        {
            Vector3 destination = Vector3.zero;

            if (transitionPackConfig.HaveMovement)
            {
                switch (transitionPackConfig.PositionMoveTypeEnum)
                {
                    case TransformEffectActionConfig.PositionMoveType.Local:
                        destination = transform.localPosition + transitionPackConfig.MoveOffSet;
                        break;
                    case TransformEffectActionConfig.PositionMoveType.Word:
                        destination = transform.position + transitionPackConfig.MoveOffSet;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return transform.Transition(destination, transitionPackConfig, onComplete);
        }

        public static Sequence Transition(this Transform transform, Transform destination,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null)
        {
            Sequence sequence = DOTween.Sequence();

            if (transitionPackConfig.HaveMovement)
                sequence.Join(transform.DoMove(destination.position, transitionPackConfig));

            sequence.Join(transform.Scale(destination.localScale, transitionPackConfig));

            if (transitionPackConfig.HaveRotation)
                sequence.Join(transform.Rotate(destination.rotation.eulerAngles, transitionPackConfig.Rotation));

            if (onComplete != null)
                sequence.OnComplete(onComplete.Invoke);

            return sequence;
        }

        public static Sequence Transition(this Transform transform, Vector3 destination,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null)
        {
            Sequence sequence = DOTween.Sequence();

            if (transitionPackConfig.HaveMovement)
                sequence.Join(transform.DoMove(destination, transitionPackConfig));

            if (transitionPackConfig.HaveScale)
            {
                switch (transitionPackConfig.ScaleType)
                {
                    case TransformEffectActionConfig.ScaleTypeEnum.ByFloat:
                        sequence.Join(transform.Scale(transitionPackConfig.ScaleMultiplier, transitionPackConfig));
                        break;
                    case TransformEffectActionConfig.ScaleTypeEnum.ByVector:
                        sequence.Join(transform.Scale(transitionPackConfig.ScaleVector, transitionPackConfig));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (transitionPackConfig.HaveRotation)
                sequence.Join(transform.Rotate(transitionPackConfig.Rotate, transitionPackConfig.Rotation));

            if (onComplete != null)
                sequence.OnComplete(onComplete.Invoke);

            return sequence;
        }

        #endregion

        #region SetPositionAndScale

        public static Sequence SetPosition(this Transform transform, Transform destination, Action onComplete = null)
        {
            return transform.SetPosition(destination.position, onComplete);
        }

        public static Sequence SetPosition(this Transform transform, Vector3 destination, Action onComplete = null)
        {
            return transform.DoMove(destination, null, onComplete);
        }

        public static Tween SetScale(this Transform transform, float scale, Action onComplete = null)
        {
            return transform.Scale(scale, 0, null, onComplete);
        }

        // public static Tween SetScale(this Transform transform ,Vector3 scale, Action onComplete = null)
        // {
        //     return transform.Scale(scale,null,onComplete);
        // }

        #endregion

        #region Move

        public static Sequence Move(this Transform transform, TransformEffectActionConfig transitionPackConfig,
            Action onComplete = null)
        {
            Vector3 destination = Vector3.zero;

            switch (transitionPackConfig.PositionMoveTypeEnum)
            {
                case TransformEffectActionConfig.PositionMoveType.Local:
                    destination = transform.localPosition + transitionPackConfig.MoveOffSet;
                    break;
                case TransformEffectActionConfig.PositionMoveType.Word:
                    destination = transform.position + transitionPackConfig.MoveOffSet;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return transform.Move(destination, transitionPackConfig, onComplete);
        }

        public static Sequence Move(this Transform transform, Transform destination,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null)
        {
            return transform.Move(destination.position, transitionPackConfig, onComplete);
        }

        public static Sequence Move(this Transform transform, Vector3 destination,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null)
        {
            return transform.DoMove(destination, transitionPackConfig, onComplete);
        }

        #endregion

        #region Scale

        public static Tween Scale(this Transform transform, TransformEffectActionConfig transitionPackConfig,
            Action onComplete = null) //transitionPackConfig scale input
        {
            switch (transitionPackConfig.ScaleType)
            {
                case TransformEffectActionConfig.ScaleTypeEnum.ByFloat:
                    return transform.Scale(transitionPackConfig.ScaleMultiplier, transitionPackConfig, onComplete);
                case TransformEffectActionConfig.ScaleTypeEnum.ByVector:
                    return transform.Scale(transitionPackConfig.ScaleVector, transitionPackConfig, onComplete);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Tween Scale(this Transform transform, float multiply,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null) //multiply scale input
        {
            return transform.Scale(multiply, transitionPackConfig.Scale, onComplete);
        }

        public static Tween Scale(this Transform transform, Vector3 scaleVector,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null) //scaleVector scale input
        {
            return transform.Scale(scaleVector, transitionPackConfig.Scale, onComplete);
        }

        #endregion

        #region Rotation

        public static Tween Rotate(this Transform transform, TransformEffectActionConfig transitionPackConfig,
            Action onComplete = null)
        {
            return transform.Rotate(transitionPackConfig.Rotate, transitionPackConfig, onComplete);
        }

        public static Tween Rotate(this Transform transform, Vector3 destination,
            TransformEffectActionConfig transitionPackConfig, Action onComplete = null)
        {
            return transform.Rotate(destination, transitionPackConfig.Rotation, onComplete);
        }

        #endregion

        #endregion

        #region PrivateFunction

        #region Move

        private static Sequence DoMove(this Transform transform, Vector3 destination,
            TransformEffectActionConfig transitionPackConfig = null,
            Action onComplete = null)
        {
            Transition3D param = null;

            if (transitionPackConfig != null)
            {
                param = transitionPackConfig.Movement;
            }

            switch (transitionPackConfig != null
                        ? transitionPackConfig.PositionMoveTypeEnum
                        : (TransformEffectActionConfig.PositionMoveType?)null)
            {
                case TransformEffectActionConfig.PositionMoveType.Local:
                    return transform.MoveLocalPosition(destination, param?.TimeToTransition ?? 0,
                        param?.AnimationCurveX, param?.AnimationCurveY,
                        param?.AnimationCurveZ,
                        onComplete);
                case TransformEffectActionConfig.PositionMoveType.Word:
                    return transform.MoveWordPosition(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
                        param?.AnimationCurveY,
                        param?.AnimationCurveZ,
                        onComplete);
                case null:
                    return transform.MoveWordPosition(destination, 0, null, null, null, onComplete);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Sequence MoveWordPosition(this Transform transform, Vector3 destination, float timeToTransition,
            AnimationCurve animationCurveX = null,
            AnimationCurve animationCurveY = null, AnimationCurve animationCurveZ = null, Action onComplete = null)
        {
            if (timeToTransition == 0)
            {
                transform.position = destination;

                onComplete?.Invoke();
                return null;
            }

            var sequence = DOTween.Sequence();

            Tween tweenX;
            Tween tweenY;
            Tween tweenZ = null;

            sequence.Join(tweenX = transform.DOMoveX(destination.x, timeToTransition));
            sequence.Join(tweenY = transform.DOMoveY(destination.y, timeToTransition));

            if (destination.z != 0) sequence.Join(tweenZ = transform.DOMoveZ(destination.z, timeToTransition));

            if (animationCurveX != null) tweenX.SetEase(animationCurveX);

            if (animationCurveY != null) tweenY.SetEase(animationCurveY);

            if (tweenZ != null)
                if (animationCurveZ != null)
                    tweenZ.SetEase(animationCurveZ);


            if (onComplete != null) sequence.OnComplete(() => onComplete?.Invoke());

            return sequence;
        }

        private static Sequence MoveLocalPosition(this Transform transform, Vector3 destination, float timeToTransition,
            AnimationCurve animationCurveX = null,
            AnimationCurve animationCurveY = null, AnimationCurve animationCurveZ = null, Action onComplete = null)
        {
            if (timeToTransition == 0)
            {
                transform.localPosition = destination;

                onComplete?.Invoke();
                return null;
            }

            var sequence = DOTween.Sequence();

            Tween tweenX;
            Tween tweenY;
            Tween tweenZ = null;

            sequence.Join(tweenX = transform.DOLocalMoveX(destination.x, timeToTransition));
            sequence.Join(tweenY = transform.DOLocalMoveY(destination.y, timeToTransition));

            if (destination.z != 0) sequence.Join(tweenZ = transform.DOLocalMoveZ(destination.z, timeToTransition));


            if (animationCurveX != null) tweenX.SetEase(animationCurveX);

            if (animationCurveY != null) tweenY.SetEase(animationCurveY);

            if (tweenZ != null)
                if (animationCurveZ != null)
                    tweenZ.SetEase(animationCurveZ);


            if (onComplete != null) sequence.OnComplete(() => onComplete?.Invoke());

            return sequence;
        }

        #endregion

        #region Scale

        private static Tween Scale(this Transform transform, float scaleMultiplier, Transition3D param = null,
            Action onComplete = null)
        {
            return transform.Scale(scaleMultiplier, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
                onComplete);
        }

        private static Sequence Scale(this Transform transform, Vector3 scaleVector, Transition3D param,
            Action onComplete = null)
        {
            return transform.Scale(scaleVector, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
                param?.AnimationCurveY,
                param?.AnimationCurveZ,
                onComplete);
        }

        private static Sequence Scale(this Transform transform, Vector3 scaleVector, float timeToTransition,
            AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
            AnimationCurve animationCurveZ = null, Action onComplete = null) //vector scale
        {
            if (timeToTransition == 0)
            {
                transform.localScale = scaleVector;

                onComplete?.Invoke();
                return null;
            }

            var sequence = DOTween.Sequence();

            Tween tweenX;
            Tween tweenY;
            Tween tweenZ = null;


            sequence.Join(tweenX = transform.DOScaleX(scaleVector.x, timeToTransition));
            sequence.Join(tweenY = transform.DOScaleY(scaleVector.y, timeToTransition));

            if (scaleVector.z != 0) sequence.Join(tweenZ = transform.DOScaleZ(scaleVector.z, timeToTransition));

            if (animationCurveX != null) tweenX.SetEase(animationCurveX);

            if (animationCurveY != null) tweenY.SetEase(animationCurveY);

            if (tweenZ != null)
                if (animationCurveZ != null)
                    tweenZ.SetEase(animationCurveZ);


            if (onComplete != null) tweenX.OnComplete(() => onComplete?.Invoke());

            return sequence;
        }

        private static Tween Scale(this Transform transform, float scaleMultiplier, float timeToTransition,
            AnimationCurve animationCurveX = null, Action onComplete = null) //scaleMultiplier scale
        {
            if (timeToTransition == 0)
            {
                transform.localScale *= scaleMultiplier;

                onComplete?.Invoke();
                return null;
            }

            Tween tweenX = transform.DOScale(Vector3.one * scaleMultiplier, timeToTransition);

            if (animationCurveX != null) tweenX.SetEase(animationCurveX);


            if (onComplete != null) tweenX.OnComplete(() => onComplete?.Invoke());

            return tweenX;
        }

        #endregion

        #region Rotate

        private static Tween Rotate(this Transform transform, Vector3 destination, Transition3D param,
            Action onComplete = null)
        {
            return transform.Rotate(destination, param?.TimeToTransition ?? 0, param?.AnimationCurveX,
                param?.AnimationCurveY,
                param?.AnimationCurveZ, onComplete);
            ;
        }

        private static Tween Rotate(this Transform transform, Vector3 destination, float timeToTransition,
            AnimationCurve animationCurveX = null, AnimationCurve animationCurveY = null,
            AnimationCurve animationCurveZ = null, Action onComplete = null)
        {
            if (timeToTransition == 0)
            {
                transform.rotation = Quaternion.Euler(destination);

                onComplete?.Invoke();
                return null;
            }


            Tween tween = transform.DORotate(destination, timeToTransition);

            if (animationCurveZ != null) tween.SetEase(animationCurveZ);

            if (onComplete != null) tween.OnComplete(() => onComplete?.Invoke());

            return tween;
        }

        #endregion

        #endregion

        #region TweenManagnent

        private static void Kill(ref Tween tween)
        {
            if (tween != null) tween.Kill();
        }

        private static void Kill(ref Sequence sequence)
        {
            if (sequence != null) sequence.Kill();
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

#region HelperClass

public static class RectTransformHelper
{
    public static Vector2 GetLocalPosition(this RectTransform rectTransform)
    {
        return rectTransform.transform.localPosition;
    }

    public static Vector2 GetWorldPosition(this RectTransform rectTransform)
    {
        return rectTransform.transform.TransformPoint(rectTransform.rect.center);
    }
}

#endregion