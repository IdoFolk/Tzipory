using UnityEngine;

namespace Tzipory.Systems.StatusSystem
{
    public abstract class BaseEffectActionConfig : ScriptableObject
    {
        public abstract EffectActionType ActionType { get; }
    }

    public enum EffectActionType
    {
        Transform,
        Color,
        Outline,
        ParticleEffects,
        Sound,
        Sprite,
    }
}