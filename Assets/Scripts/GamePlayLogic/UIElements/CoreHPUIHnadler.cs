namespace GameplayeLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {
        public override void Show()
        {
            LevelManager.CoreTemplete.Health.OnValueChanged += UpdateUiData;
            _maxCount.text = $"/{LevelManager.CoreTemplete.Health.BaseValue}";
            UpdateUiData(LevelManager.CoreTemplete.Health.CurrentValue);
            base.Show();
        }

        public override void Hide()
        {
            LevelManager.CoreTemplete.Health.OnValueChanged -= UpdateUiData;
            base.Hide();
        }
    }
}