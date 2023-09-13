using Systems.UISystem;
using TMPro;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenWaveUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _text;

        public override void Show()
        {
            _text.text = $"{LevelManager.WaveManager.WaveNumber}/{LevelManager.WaveManager.TotalNumberOfWaves}";
            base.Show();
        }
    }
}