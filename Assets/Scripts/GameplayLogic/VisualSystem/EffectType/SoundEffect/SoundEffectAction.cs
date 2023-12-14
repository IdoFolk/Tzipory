using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.StatusSystem.EffectActionTypeSO;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    public class SoundEffectAction : BaseEffectAction , IPoolable<SoundEffectAction>
    {
        private AudioClip[] _audioClips;
        
        private bool _randomPitch = false;
        private bool _randomVolume = false;
        
        private Vector2 _volume;
        private Vector2 _pitchRange;

        private AudioClip _selectedAudioClip;

        public override float Duration => _selectedAudioClip.length;

        public override void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            base.Init(actionContainerConfig, visualComponent);
            
            var config = GetConfig<SoundEffectActionConfig>(actionContainerConfig.EffectActionConfig);

            _audioClips = config.AudioClips;
            _volume = config.VolumeRange;
            _randomPitch  = config.RandomPitch;
            _pitchRange = config.PitchRange;
            _randomVolume = config.RandomVolume;
        }

        public override void StartEffectAction()
        {
            float pitch = 1;
            float volume = 1;
            
            if (_randomPitch)
                pitch = Random.Range(_pitchRange.x, _pitchRange.y);
            if (_randomVolume)
                volume  = Random.Range(_volume.x, _volume.y);

            _selectedAudioClip = _audioClips[Random.Range(0, _audioClips.Length)];

            //VisualComponent.SoundHandler.PlayAudioClip(_selectedAudioClip,pitch ,volume);
        }

        public override void ProcessEffectAction()
        {
        }

        public override void CompleteEffectAction()
        {
        }

        public override void UndoEffectAction()
        {
        }

        public override void InterruptEffectAction()
        {
        }

        public event Action<SoundEffectAction> OnDispose;
        public void Dispose() => OnDispose?.Invoke(this);
        
        public void Free()
        {
            _audioClips = null;
            _selectedAudioClip = null;
        }
    }
}