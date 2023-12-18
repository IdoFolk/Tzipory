using UnityEditor.Animations;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct AnimatorComponentConfig
    {
        [SerializeField] public AnimatorComponentType AnimatorType;
        [SerializeField] public AnimatorController EntityAnimator;
    }

    public enum AnimatorComponentType
    {
        Hero,
        Enemy,
    }
}