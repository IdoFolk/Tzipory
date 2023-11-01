using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using UnityEngine;

namespace Tzipory.Testing
{
    [CreateAssetMenu()]
    public class LevelVisualDataSO : ScriptableObject
    {
        public LevelVisualData level_VisualData;
        public PopUpTextConfig DefaultPopUpConfig;
    }
}