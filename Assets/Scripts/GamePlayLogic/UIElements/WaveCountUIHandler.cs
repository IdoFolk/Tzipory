

namespace GameplayeLogic.UIElements
{
    public class WaveCountUIHandler : BaseCounterUIHandler
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