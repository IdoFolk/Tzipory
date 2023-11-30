using System;
using Tzipory.Tools.Interface;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    [Serializable]
    public class PostProcessSlowMotionEffect : EffectTransitionLerp<PostProcessType> , IInitialization<Volume>
    {
        private Volume _postProcessVolume;
        
        public bool IsInitialization { get; }
        public void Init(Volume postProcessVolume)
        {
            _postProcessVolume = postProcessVolume;
        }

        protected override void SetValue(PostProcessType type, float value)
        {
            switch (type)
            {
                case PostProcessType.Bloom:
                    if (_postProcessVolume.profile.TryGet<Bloom>(out var bloom))
                    {
                        bloom.intensity.value = value;
                    }
                    break;
                case PostProcessType.Vignette:
                    if (_postProcessVolume.profile.TryGet<Vignette>(out var vignette))
                    {
                        vignette.intensity.value = value;
                    }
                    break;
            }       
        }
    }
    public enum PostProcessType
    {
        Bloom,
        Vignette
    }
}