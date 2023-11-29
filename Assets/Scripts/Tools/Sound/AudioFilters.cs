using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Tools.Sound
{
    public abstract class AudioFilters : MonoBehaviour
    {
        protected const string AUDIO_LOG_GROUP = "Audio";
        [SerializeField] protected AudioSource _audioSource;
        [SerializeField,ShowIf(nameof(_audioReverbFilterEnabled))] protected AudioReverbFilter _audioReverbFilter;
        [SerializeField,ShowIf(nameof(_audioLowPassFilterEnabled))] protected AudioLowPassFilter _audioLowPassFilter;
        [Header("Slow Motion Effect Audio Filters")] [SerializeField, TabGroup("Slow Motion Effects")] protected AudioFilter[] _audioFilters;

        private bool _audioReverbFilterEnabled;
        private bool _audioLowPassFilterEnabled;
        protected virtual void OnValidate()
        {
            _audioSource ??= TryGetComponent<AudioSource>(out var audioSource)
                ? audioSource
                : gameObject.AddComponent<AudioSource>();

            if (_audioFilters is not null)
            {
                foreach (var audioFilter in _audioFilters)
                {
                    switch (audioFilter.Type)
                    {
                        case AudioFilterType.LowPass:
                            _audioLowPassFilterEnabled = audioFilter.Enabled;
                            break;
                        case AudioFilterType.Reverb:
                            _audioReverbFilterEnabled = audioFilter.Enabled;
                            break;
                    }
                }
            }
            
            if (_audioReverbFilterEnabled)
            {
                gameObject.AddComponent<AudioReverbFilter>();
                _audioReverbFilter ??= GetComponent<AudioReverbFilter>();
            }
            if (_audioLowPassFilterEnabled)
            {
                gameObject.AddComponent<AudioLowPassFilter>();
                _audioLowPassFilter ??= GetComponent<AudioLowPassFilter>();
            }
        }
        
        public bool SetAudioFilterValue(AudioFilterValue audioFilter, float value)
        {
            switch (audioFilter.Name)
            {
                case AudioFilterValueType.cutoffFrequency:
                    if (!_audioLowPassFilterEnabled) return false;
                    _audioLowPassFilter.cutoffFrequency = value;
                    return true;
                case AudioFilterValueType.lowpassResonanceQ:
                    if (!_audioLowPassFilterEnabled) return false;
                    _audioLowPassFilter.lowpassResonanceQ = value;
                    return true;
                case AudioFilterValueType.revebLevel:
                    if (!_audioReverbFilter) return false;
                    _audioReverbFilter.reverbLevel = value;
                    return true;
                case AudioFilterValueType.dryLevel:
                    if (!_audioReverbFilter) return false;
                    _audioReverbFilter.dryLevel = value;
                    return true;
                case AudioFilterValueType.pitch:
                    if (!_audioSource) return false;
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
        public bool Enabled;
        public AudioFilterType Type;
        public AudioFilterValue[] AudioFilterValues;
    }
    [Serializable]
    public struct AudioFilterValue
    {
        public AudioFilterValueType Name;
        public float DefaultValue;
        public float SlowMotionValue;
    }
    public enum AudioFilterType
    {
        AudioSource,
        Reverb,
        LowPass
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