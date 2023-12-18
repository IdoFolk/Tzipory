using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct AnimatorComponentConfig
    {
        [SerializeField] public AnimatorComponentType AnimatorType;
        [SerializeField] public AnimatorController EntityAnimator;
        [SerializeField, ShowIf(nameof(AnimatorType),AnimatorComponentType.Hero)] public GameObject AbilityCastAnimationPrefab;

    }

    public enum AnimatorComponentType
    {
        Hero,
        Enemy,
    }
}