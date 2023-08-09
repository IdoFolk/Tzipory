using UnityEngine;
using UnityEngine.Serialization;

namespace Tzipory.Tools.Sound
{
    public class SoundHandler : MonoBehaviour
    {
        [FormerlySerializedAs("_audioSource")] [SerializeField] private AudioSource[] _audioSources;

        public void PlayAudioClip(AudioClip audioClip,float pitch,float volume, bool loop = false)
        {
            AudioSource audioSources = null;

            foreach (var source in _audioSources)
            {
                if (source.isPlaying) continue;
                
                audioSources = source;
                break;
            }
            
            if (audioSources == null)
            {
                Debug.LogWarning("did not find any available audio source");
                return;
            }
            
            audioSources.clip = audioClip;
            audioSources.loop = loop;
            audioSources.pitch = pitch;
            audioSources.volume = volume;
            audioSources.Play();
        }

        private void OnValidate()
        {
            if (_audioSources == null || _audioSources.Length == 0)
            {
                _audioSources = GetComponents<AudioSource>();
            }
        }
    }
}