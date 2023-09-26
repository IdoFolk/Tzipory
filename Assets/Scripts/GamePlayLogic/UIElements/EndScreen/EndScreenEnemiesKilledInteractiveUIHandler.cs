using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenEnemiesKilledInteractiveUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private TMP_Text _countText;

        public override void Show()
        {
            _countText.text = LevelManager.EnemyManager.NumberOfEnemiesKilled.ToString();
            base.Show();
        }
    }
}