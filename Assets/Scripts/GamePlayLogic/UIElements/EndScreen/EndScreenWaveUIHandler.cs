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
            _text.text = $"{GameManager.WaveManager.WaveNumber}/{GameManager.WaveManager.TotalNumberOfWaves}";
            base.Show();
        }
    }
}