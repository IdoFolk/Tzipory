using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.Tools.Sound
{
    public class BgMusicManager : AudioFilters
    {
        #region Singleton

        private static BgMusicManager _instance;
        public static BgMusicManager Instance => _instance;

        public virtual void Awake()
        {
            if (isActiveAndEnabled)
            {
                if (Instance == null)
                    _instance = this;
                else if (Instance != this)
                    Destroy(this);
            }
        }

        #endregion


        

        private static AnimationCurve _defaultCurve = AnimationCurve.Linear(0, 0, 1, 1);


        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        private void Start()
        {
            SetDefaultEffect();
        }

        public void PlayMusic()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            if (!_audioSource.isPlaying) return;
            _audioSource.Stop();
        }

        public void ChangeMusicVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public void SetSlowMotionEffect(float transitionTime = 1, AnimationCurve curve = null)
        {
            if (_audioFilters is null) return;
            foreach (var audioFilter in _audioFilters)
            {
                foreach (var value in audioFilter.AudioFilterValues)
                {
                    StartCoroutine(FadeMusic(value, value.DefaultValue, value.SlowMotionValue,
                        transitionTime, curve));
                }
            }
        }

        public void SetDefaultEffect(float transitionTime = 1, AnimationCurve curve = null)
        {
            if (_audioFilters is null) return;
            foreach (var audioFilter in _audioFilters)
            {
                foreach (var value in audioFilter.AudioFilterValues)
                {
                    StartCoroutine(FadeMusic(value, value.SlowMotionValue, value.DefaultValue,
                        transitionTime, curve));
                }
            }
        }

        private IEnumerator FadeMusic(AudioFilterValue audioFilter, float oldValue, float newValue, float transitionTime,
            AnimationCurve curve)
        {
            float transitionTimeCount = 0;
            var animationCurve = curve ?? _defaultCurve;

            while (transitionTimeCount < transitionTime)
            {
                transitionTimeCount += Time.deltaTime;

                float evaluateValue = animationCurve.Evaluate(transitionTimeCount / transitionTime);

                float value = Mathf.Lerp(oldValue, newValue, evaluateValue);
                SetAudioFilterValue(audioFilter, value);

                yield return null;
            }

            SetAudioFilterValue(audioFilter, newValue);
        }
    }
}