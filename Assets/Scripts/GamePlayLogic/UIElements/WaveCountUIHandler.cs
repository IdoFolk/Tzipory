

namespace GameplayeLogic.UIElements
{
    public class WaveCountUIHandler : BaseCounterUIHandler
    {
        public override void Show()
        {
            _maxCount.text = $"/{GameManager.WaveManager.TotalNumberOfWaves}";
            GameManager.WaveManager.OnNewWaveStarted += UpdateUiData;
            base.Show();
        }

        public override void Hide()
        {
            GameManager.WaveManager.OnNewWaveStarted -= UpdateUiData;
            base.Hide();
        }
    }
}