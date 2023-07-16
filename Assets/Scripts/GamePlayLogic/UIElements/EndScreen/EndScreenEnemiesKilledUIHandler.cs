using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenEnemiesKilledUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _countText;

        public override void Show()
        {
            _countText.text = GameManager.EnemyManager.NumberOfEnemiesKilled.ToString();
            base.Show();
        }
    }
}