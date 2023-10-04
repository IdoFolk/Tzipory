using TMPro;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenEnemiesKilledInteractiveUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _countText;

        public override void Show()
        {
            _countText.text = LevelManager.EnemyManager.NumberOfEnemiesKilled.ToString();
            base.Show();
        }
    }
}