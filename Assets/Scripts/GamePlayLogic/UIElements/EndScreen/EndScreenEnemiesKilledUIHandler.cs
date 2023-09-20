using Tzipory.Systems.UISystem;
using TMPro;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenEnemiesKilledUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _countText;

        public override void Show()
        {
            _countText.text = LevelManager.EnemyManager.NumberOfEnemiesKilled.ToString();
            base.Show();
        }
    }
}