using System;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem
{
    public class EntityDefaultAnimationsComponent : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        public void PlayDefaultAnimation(DefaultAnimation animation)
        {
            _animator.Play(animation.ToString());
        }
    }

    public enum DefaultAnimation
    {
        Attack,
        TakeDamage,
        Walk,
        Death
    }
}