using System;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Tools.Sound
{
    public abstract class AudioFilters : MonoBehaviour
    {
        protected const string AUDIO_LOG_GROUP = "Audio";

        [SerializeField] protected AudioSource _audioSource;
        [SerializeField] protected AudioReverbFilter _audioReverbFilter;
        [SerializeField] protected AudioLowPassFilter _audioLowPassFilter;
        protected virtual void OnValidate()
        {
            _audioSource ??= GetComponent<AudioSource>();
            _audioReverbFilter ??= GetComponent<AudioReverbFilter>();
            _audioLowPassFilter ??= GetComponent<AudioLowPassFilter>();
        }
        
        public bool SetAudioFilterValue(AudioFilter audioFilter, float value)
        {
            switch (audioFilter.Name)
            {
                case AudioFilterType.cutoffFrequency:
                    _audioLowPassFilter.cutoffFrequency = value;
                    return true;
                case AudioFilterType.lowpassResonanceQ:
                    _audioLowPassFilter.lowpassResonanceQ = value;
                    return true;
                case AudioFilterType.revebLevel:
                    _audioReverbFilter.reverbLevel = value;
                    return true;
                case AudioFilterType.dryLevel:
                    _audioReverbFilter.dryLevel = value;
                    return true;
                case AudioFilterType.pitch:
                    _audioSource.pitch = value;
                    return true;
                default:
                    Logger.Log("could not find audio filter type in enum", AUDIO_LOG_GROUP);
                    return false;
            }

        }
    }
    [Serializable]
    public struct AudioFilter
    {
        public AudioFilterType Name;
        public float DefaultValue;
        public float SlowMotionValue;
    }
    public enum AudioFilterType
    {
        dryLevel = 0,
        revebLevel = 1,
        lowpassResonanceQ = 2,
        cutoffFrequency = 3,
        pitch = 4
    }
}