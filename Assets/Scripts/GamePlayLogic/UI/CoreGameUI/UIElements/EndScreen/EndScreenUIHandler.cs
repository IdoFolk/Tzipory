using TMPro;
using Tools.Enums;
using Tzipory.ConfigFiles.ToolTipConfig;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _tips;
        [SerializeField] private EndScreenTextsConfig _config;
       
        
        public override void Show()
        {
            var isWon = LevelManager.IsWon;
            _title.text = isWon ? "You won!" : "You lost!";
            _tips.text  = isWon ? _config.WinText[Random.Range(0,_config.WinText.Length)] : _config.LoseText[Random.Range(0,_config.LoseText.Length)];
            base.Show();
        }
    }
}