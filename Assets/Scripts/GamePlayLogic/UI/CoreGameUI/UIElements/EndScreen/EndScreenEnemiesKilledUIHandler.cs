using TMPro;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenEnemiesKilledUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _countText;


        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            _countText.text = LevelManager.EnemyManager.NumberOfEnemiesKilled.ToString();
        }
    }
}