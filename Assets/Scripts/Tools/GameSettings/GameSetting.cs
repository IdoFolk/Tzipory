using UnityEngine;

namespace Tzipory.Tools.GameSettings
{
    public class GameSetting : ScriptableObject , ISerializationCallbackReceiver 
    {
        [SerializeField] private bool _noWinOrLose;

        public static bool CantLose { get; private set; }
        
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            CantLose = _noWinOrLose;
        }
    }
}