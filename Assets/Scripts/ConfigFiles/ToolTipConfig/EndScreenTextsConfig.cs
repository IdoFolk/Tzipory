using UnityEngine;

namespace Tzipory.ConfigFiles.ToolTipConfig
{
    [CreateAssetMenu(fileName = "NewEndScreenTexts", menuName = "ScriptableObjects/new end screen text", order = 0)]
    public class EndScreenTextsConfig : ScriptableObject
    {
        [SerializeField] private string[] _winText;
        [SerializeField] private string[] _loseText;

        public string[] WinText => _winText;

        public string[] LoseText => _loseText;
    }
}