using Tzipory.EntitySystem.StatusSystem;

namespace GameplayeLogic.UIElements
{
    public class CoreHPUIHnadler : BaseInteractiveCounterUIHandler
    {
        public override void Show()
        {
            LevelManager.CoreTemplete.Health.OnValueChangedData += UpdateCoreUI;
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
            LevelManager.CoreTemplete.Health.OnValueChangedData -= UpdateCoreUI;
            base.Hide();
        }
    }
}