using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.ConfigFiles.Visual;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.Sound;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityVisualComponent : IEntityComponent , IInitialization<BaseGameEntity,VisualComponentConfig>
    {
        public event Action<Sprite> OnSetSprite;
        public event Action<bool> OnSpriteFlipX;
        public VisualComponentConfig VisualComponentConfig { get; }
        public EffectSequenceHandler EffectSequenceHandler { get; }
        public SpriteRenderer MainSpriteRenderer { get; }
        public SoundHandler SoundHandler { get; }//Temp need to make a sound component
        public PlayableDirector ParticleEffectPlayableDirector { get; }
        public PopUpTexter PopUpTexter { get; }

        public void StartAnimationEffect(AnimationConfig config);

        public void ResetVisual();
    }
}