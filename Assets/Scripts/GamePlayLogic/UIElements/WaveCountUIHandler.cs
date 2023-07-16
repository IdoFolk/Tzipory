

namespace GameplayeLogic.UIElements
{
    public class WaveCountUIHandler : BaseCounterUIHandler
    {
        private void Start()
        {
            Show();
        }

        public override void Show()
        {
            _maxCount.text = $"/{GameManager.LevelManager.TotalNumberOfWaves}";
            GameManager.LevelManager.OnNewWaveStarted += UpdateUiData;
            base.Show();
        }

        public override void Hide()
        {
            GameManager.LevelManager.OnNewWaveStarted -= UpdateUiData;
            base.Hide();
        }
    }
}