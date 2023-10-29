using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class WaveCountUIHandler : BaseCounterUIHandler
    {
        protected override UIGroup UIGroup => UIGroup.GameUI;
        
        public override void Show()
        {
            LevelManager.WaveManager.OnNewWaveStarted += UpdateUiData;
            base.Show();
        }

        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            _maxCount.text = $"/{LevelManager.WaveManager.TotalNumberOfWaves}";
        }

        public override void Hide()
        {
            LevelManager.WaveManager.OnNewWaveStarted -= UpdateUiData;
            base.Hide();
        }
    }
}