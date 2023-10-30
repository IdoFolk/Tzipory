using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {

        public override void Init()
        {
            _maxCount.text = $"/{LevelManager.CoreTemplete.Health.BaseValue}";
            base.Init();
        }

        public override void Show()
        {
            LevelManager.CoreTemplete.Health.OnValueChangedData += UpdateCoreUI;
            base.Show();
        }

        private void UpdateCoreUI(StatChangeData statChangeData)
        {
            UpdateUIVisual();
        }

        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            UpdateUiData(LevelManager.CoreTemplete.Health.CurrentValue);
        }

        public override void Hide()
        {
            LevelManager.CoreTemplete.Health.OnValueChangedData -= UpdateCoreUI;
            base.Hide();
        }
    }
}