using MyNamespaceTzipory.Systems.VisualSystem;
using Tzipory.Tools.Sound;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityVisualComponent : IEntityComponent
    {
        public EffectSequenceHandler EffectSequenceHandler { get; }
        public SpriteRenderer SpriteRenderer { get; }
        public SoundHandler SoundHandler { get; }
        public Transform ParticleEffectPosition { get; }
        public Transform VisualQueueEffectPosition { get; }

        public PopUpTexter PopUpTexter { get; }
    }
}