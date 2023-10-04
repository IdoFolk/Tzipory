using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class WaveCountUIHandler : BaseInteractiveCounterUIHandler
    {
        public override void Show()
        {
            _maxCount.text = $"/{LevelManager.WaveManager.TotalNumberOfWaves}";
            LevelManager.WaveManager.OnNewWaveStarted += UpdateUiData;
            base.Show();
        }

        public override void Hide()
        {
            LevelManager.WaveManager.OnNewWaveStarted -= UpdateUiData;
            base.Hide();
        }
    }
}