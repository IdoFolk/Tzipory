using ConfigFiles;
using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenInteractiveUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _tips;
        [SerializeField] private EndScreenTextsConfig _config;
        [SerializeField] private EndScreenEnemiesKilledInteractiveUIHandler  _enemiesKilledInteractiveHandler;
        [SerializeField] private EndScreenTimeInteractiveUIHandler _timeInteractiveHandler;
        [SerializeField] private EndScreenWaveInteractiveUIHandler _waveInteractiveUIHandler;

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
            _enemiesKilledInteractiveHandler.Show();
            _timeInteractiveHandler.Show();
            _waveInteractiveUIHandler.Show();
        }
    }
}