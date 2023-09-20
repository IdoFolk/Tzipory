using Tzipory.ConfigFiles.ToolTipConfig;
using Tzipory.Systems.UISystem;
using TMPro;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenUiHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _tips;
        [SerializeField] private EndScreenTextsConfig _config;
        [SerializeField] private EndScreenEnemiesKilledUIHandler  _enemiesKilledHandler;
        [SerializeField] private EndScreenTimeUIHandler _timeHandler;
        [SerializeField] private EndScreenWaveUIHandler _waveUIHandler;

        private void Awake()
        {
            LevelManager.OnEndGame += OpenEndScreen;
            Hide();//temp
        }

        private void OnDestroy()
        {
            LevelManager.OnEndGame -= OpenEndScreen;
        }

        private void OpenEndScreen(bool isWon)
        { 
            _title.text = isWon ? "You won!" : "You lost!";
            _tips.text  = isWon ? _config.WinText[Random.Range(0,_config.WinText.Length)] : _config.LoseText[Random.Range(0,_config.LoseText.Length)];
            Show();
        }

        private void OnEnable()
        {
            _enemiesKilledHandler.Show();
            _timeHandler.Show();
            _waveUIHandler.Show();
        }
    }
}