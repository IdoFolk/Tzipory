using Tzipory.ConfigFiles.Visual;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityVisualComponent : IEntityComponent
    {
        public EffectSequenceHandler EffectSequenceHandler { get; }
        public SpriteRenderer SpriteRenderer { get; }
        public PlayableDirector ParticleEffectPlayableDirector { get; }
        public PopUpTexter PopUpTexter { get; }

        public void StartAnimationEffect(AnimationConfig config);

        public void ResetVisual();
    }
}