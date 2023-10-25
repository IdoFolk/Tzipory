using TMPro;
using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenEnemiesKilledInteractiveUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _countText;

        protected override UIGroupType GroupIndex => UIGroupType.EndGameUI;

        public override void Show()
        {
            _countText.text = LevelManager.EnemyManager.NumberOfEnemiesKilled.ToString();
            base.Show();
        }
    }
}