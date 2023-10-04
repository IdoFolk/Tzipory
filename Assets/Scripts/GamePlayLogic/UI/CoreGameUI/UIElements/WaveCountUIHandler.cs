using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class WaveCountUIHandler : BaseCounterUIHandler
    {
        protected override void Awake()
        {
            UIManager.AddObserverObject(this);
        }
        
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