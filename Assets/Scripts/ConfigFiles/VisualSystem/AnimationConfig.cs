using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Tzipory.ConfigFiles.Visual
{
    [System.Serializable]
    public struct AnimationConfig
    {
        private int _currentTimeLine;
        
        [SerializeField] public bool HaveEnterAndExit;
        
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public PlayableDirector[] _entryTimeLine;
        [SerializeField] public PlayableDirector[] _loopTimeLine;
        [SerializeField,ShowIf(nameof(HaveEnterAndExit))] public PlayableDirector[] _exitTimeLine;

        public float EntryTime => (float)EntryTimeLine.duration;
        public float LoopTime;
        public float ExitTime => (float)ExitTimeLine.duration;

        public PlayableDirector EntryTimeLine
        {
            get
            {
                _currentTimeLine = Random.Range(0, _entryTimeLine.Length);
                
                return  _entryTimeLine[1];
            }
        }

        public PlayableDirector LoopTimeLine => _loopTimeLine[1];
        public PlayableDirector ExitTimeLine => _exitTimeLine[1];
    }
}