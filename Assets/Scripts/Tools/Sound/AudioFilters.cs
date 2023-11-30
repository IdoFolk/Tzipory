using System;
using Tzipory.GameplayLogic.VisualSystem.EffectType;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.Tools.Sound
{
    [Serializable]
    public class AudioFilters : EffectTransitionLerp<AudioFilterValueType> , IInitialization<AudioSource,AudioReverbFilter,AudioLowPassFilter>
    {
        protected AudioSource _audioSource;
        protected AudioReverbFilter _audioReverbFilter;
        protected AudioLowPassFilter _audioLowPassFilter;
        
        public bool IsInitialization { get; }
        public void Init(AudioSource audioSource, AudioReverbFilter audioReverbFilter,AudioLowPassFilter audioLowPassFilter)
        {
            _audioSource = audioSource;
            _audioReverbFilter = audioReverbFilter;
            _audioLowPassFilter = audioLowPassFilter;
        }

        protected override void SetValue(AudioFilterValueType type, float value)
        {
            switch (type)
            {
                case AudioFilterValueType.cutoffFrequency:
                    _audioLowPassFilter.cutoffFrequency = value;
                    break;
                case AudioFilterValueType.lowpassResonanceQ:
                    _audioLowPassFilter.lowpassResonanceQ = value;
                    break;
                case AudioFilterValueType.revebLevel:
                    _audioReverbFilter.reverbLevel = value;
                    break;
                case AudioFilterValueType.dryLevel:
                    _audioReverbFilter.dryLevel = value;
                    break;
                case AudioFilterValueType.pitch:
                    _audioSource.pitch = value;
                    break;
            }
        }
    }

    public enum AudioFilterValueType
    {
        dryLevel = 0,
        revebLevel = 1,
        lowpassResonanceQ = 2,
        cutoffFrequency = 3,
        pitch = 4
    }
}