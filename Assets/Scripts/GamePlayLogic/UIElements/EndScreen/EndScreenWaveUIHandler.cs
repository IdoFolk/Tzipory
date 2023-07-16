using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenWaveUIHandler : BaseUIElement
    {
        [SerializeField] private TMP_Text _text;

        public override void Show()
        {
            _text.text = $"{GameManager.LevelManager.WaveNumber}/{GameManager.LevelManager.TotalNumberOfWaves}";
            base.Show();
        }
    }
}