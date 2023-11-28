using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.Helpers;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

public class BgMusicManager : MonoSingleton<BgMusicManager> //temp singleton
{
    private const string AUDIO_LOG_GROUP = "Audio";
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioReverbFilter _audioReverbFilter;
    [SerializeField] private AudioLowPassFilter audioLowPassFilter;
    [Header("Slow Motion Effect Audio Filters")]
    [SerializeField,TabGroup("Slow Motion Effects")] private AudioFilter[] _serializedAudioFilters;


    private void OnValidate()
    {
        _audioSource ??= GetComponent<AudioSource>();
        _audioReverbFilter ??= GetComponent<AudioReverbFilter>();
        audioLowPassFilter ??= GetComponent<AudioLowPassFilter>();
    }

    private void Start()
    {
        SetDefaultEffect();
    }
    public void SetSlowMotionEffect()
    {
        if (_serializedAudioFilters is null) return;
        foreach (var audioFilter in _serializedAudioFilters)
        {
            SetAudioFilter(audioFilter,audioFilter.SlowMotionValue);
        //add lerp
        }
    }
    public void SetDefaultEffect()
    {
        if (_serializedAudioFilters is null) return;
        foreach (var audioFilter in _serializedAudioFilters)
        {
            SetAudioFilter(audioFilter,audioFilter.DefaultValue);
            //add lerp
        }
    }

    private bool SetAudioFilter(AudioFilter audioFilter, float value)
    {
        switch (audioFilter.Name)
        {
            case AudioFilterType.cutoffFrequency:
                audioLowPassFilter.cutoffFrequency = value;
                return true;
            case AudioFilterType.lowpassResonanceQ:
                audioLowPassFilter.lowpassResonanceQ = value;
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
internal struct AudioFilter
{
    public AudioFilterType Name;
    public float DefaultValue;
    public float SlowMotionValue;
}

enum AudioFilterType
{
    none = 0,
    dryLevel = 1,
    revebLevel = 2,
    lowpassResonanceQ = 3,
    cutoffFrequency = 4,
    pitch = 5
}