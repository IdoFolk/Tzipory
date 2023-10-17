using Spine;
using Spine.Unity;
using Tzipory.Tools.Interface;
using UnityEngine;
using AnimationState = Spine.AnimationState;
using Event = Spine.Event;

namespace Tzipory.Systems.AnimationSystem
{
    public class AnimationHandler : MonoBehaviour, IInitialization<SkeletonDataAsset>
    {
        //const
        private const string IDLE = "Idle";
        private const string RUNNING = "Running";
        private const string BASIC_ATTACK = "Basic Attack2";
        private const string CRIT_ATTACK = "Basic Attack";
        private const string ABILITY_ATTACK = "AOE Attack2";
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        public AnimationState AnimationState => _skeletonAnimation.AnimationState;
        public AnimationStates CurrentAnimationStateType { get; private set; } 


        private void OnValidate()
        {
            _skeletonAnimation ??= GetComponent<SkeletonAnimation>();
        }

        public bool IsInitialization { get; private set; }
        public void Init(SkeletonDataAsset parameter)
        {
            _skeletonAnimation.skeletonDataAsset= parameter;
            TEMP_SetAnimation(AnimationStates.Idle);
            IsInitialization = true;
        }

        public TrackEntry TEMP_SetAnimation(AnimationStates animationStates)
        {
            switch (animationStates)
            {
                case AnimationStates.Idle:
                    CurrentAnimationStateType = AnimationStates.Idle;
                    return _skeletonAnimation.AnimationState.SetAnimation(0, IDLE,true);
                case AnimationStates.Running:
                    CurrentAnimationStateType = AnimationStates.Running;
                    return _skeletonAnimation.AnimationState.SetAnimation(0, RUNNING,true);
                case AnimationStates.BasicAttack:
                    return _skeletonAnimation.AnimationState.SetAnimation(1, BASIC_ATTACK,false);
                case AnimationStates.CritAttack:
                    return _skeletonAnimation.AnimationState.SetAnimation(1, CRIT_ATTACK,false);
                case AnimationStates.AbilityAttack:
                    return _skeletonAnimation.AnimationState.SetAnimation(1, ABILITY_ATTACK,false);
                case AnimationStates.Hit:
                    break;
                case AnimationStates.Death:
                    break;
            }
            return null;
        }

        public void FlipSkeletonAnimation(bool flip)
        {
            _skeletonAnimation.skeleton.FlipX = flip;
        }
    }

    public enum AnimationStates
    {
        Idle,
        Running,
        BasicAttack,
        CritAttack,
        AbilityAttack,
        Hit,
        Death
    }
}