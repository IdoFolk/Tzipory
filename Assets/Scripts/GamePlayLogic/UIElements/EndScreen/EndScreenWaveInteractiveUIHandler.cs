using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenWaveInteractiveUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private TMP_Text _text;

        public override void Show()
        {
            _text.text = $"{LevelManager.WaveManager.WaveNumber}/{LevelManager.WaveManager.TotalNumberOfWaves}";
            base.Show();
        }
    }
}