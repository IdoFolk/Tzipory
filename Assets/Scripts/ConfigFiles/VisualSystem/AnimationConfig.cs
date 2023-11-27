using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace Tzipory.ConfigFiles.Visual
{
    [System.Serializable]
    public struct AnimationConfig
    {
        [SerializeField] public bool HaveEnterAndExit;
        
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public TimelineAsset EntryTimeLine;
        [SerializeField] public TimelineAsset LoopTimeLine;
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public TimelineAsset ExitTimeLine;

        public float EntryTime => (float)EntryTimeLine.duration;
        public float LoopTime;
        public float ExitTime => (float)ExitTimeLine.duration;
    }
}