using System;
using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenUiHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private EndScreenEnemiesKilledUIHandler  _enemiesKilledHandler;
        [SerializeField] private EndScreenTimeUIHandler _timeHandler;
        [SerializeField] private EndScreenWaveUIHandler _waveUIHandler;

        private void Awake()
        {
            GameManager.OnEndGame += OpenEndScreen;
        }

        private void OnDestroy()
        {
            GameManager.OnEndGame -= OpenEndScreen;
        }

        private void OpenEndScreen(bool isWon)
        { 
            _title.text = isWon ? "You won!" : "You lost!";
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