using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Tzipory.ConfigFiles.Visual
{
    [System.Serializable]
    public struct AnimationConfig
    {
        [SerializeField] public bool HaveEnterAndExit;
        
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public PlayableDirector EntryTimeLine;
        [SerializeField] public PlayableDirector LoopTimeLine;
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public PlayableDirector ExitTimeLine;

        public float EntryTime => (float)EntryTimeLine.duration;
        public float LoopTime;
        public float ExitTime => (float)ExitTimeLine.duration;
    }
}