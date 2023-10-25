using TMPro;
using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenWaveInteractiveUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _text;

        protected override UIGroupType GroupIndex => UIGroupType.EndGameUI;

        public override void Show()
        {
            _text.text = $"{LevelManager.WaveManager.WaveNumber}/{LevelManager.WaveManager.TotalNumberOfWaves}";
            base.Show();
        }
    }
}