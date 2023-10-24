using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {
        protected override UIGroupType UIGroup => UIGroupType.GameUI;

        public override void Show()
        {
            LevelManager.CoreTemplete.Health.OnValueChanged += UpdateCoreUI;
            _maxCount.text = $"/{LevelManager.CoreTemplete.Health.BaseValue}";
            UpdateUiData(LevelManager.CoreTemplete.Health.CurrentValue);
            base.Show();
        }

        private void UpdateCoreUI(StatChangeData statChangeData)
        {
            UpdateUiData(statChangeData.NewValue);
        }

        public override void Hide()
        {
            LevelManager.CoreTemplete.Health.OnValueChanged -= UpdateCoreUI;
            base.Hide();
        }
    }
}