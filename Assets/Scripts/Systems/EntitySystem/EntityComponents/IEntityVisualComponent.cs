using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.ConfigFiles.Visual;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityVisualComponent : IEntityComponent , IInitialization<BaseGameEntity,VisualComponentConfig>
    {
        public VisualComponentConfig VisualComponentConfig { get; }
        public EffectSequenceHandler EffectSequenceHandler { get; }
        public SpriteRenderer SpriteRenderer { get; }
        public PlayableDirector ParticleEffectPlayableDirector { get; }
        public PopUpTexter PopUpTexter { get; }

        public void StartAnimationEffect(AnimationConfig config);

        public void ResetVisual();
    }
}