using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tzipory.Systems.StatusSystem.EffectActionTypeSO
{
    [CreateAssetMenu(fileName = "NewSoundEffectAction", menuName = "ScriptableObjects/VisualSystem/EffectAction/New sound effect action")]
    public class SoundEffectActionConfig : BaseEffectActionConfig
    {
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private bool _randomPitch = false;
        [SerializeField] private bool _randomVolume = false;
        [FormerlySerializedAs("_volume")]
        [MinMaxSlider(0f,1f),ShowIf("_randomVolume")]
        [SerializeField] private Vector2 _volumeRange;
        
        [MinMaxSlider(-3f,3f),ShowIf("_randomPitch")]
        [SerializeField] private Vector2 _pitchRange;

        public AudioClip[] AudioClips => audioClips;

        public Vector2 VolumeRange => _volumeRange;

        public bool RandomVolume => _randomVolume;

        public bool RandomPitch => _randomPitch;

        public Vector2 PitchRange => _pitchRange;

        public override EffectActionType ActionType => EffectActionType.Sound;
    }
}