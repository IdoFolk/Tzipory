namespace GameplayeLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {
        public override void Show()
        {
            LevelManager.CoreTemplete.HP.OnValueChanged += UpdateUiData;
            _maxCount.text = $"/{LevelManager.CoreTemplete.HP.BaseValue}";
            UpdateUiData(LevelManager.CoreTemplete.HP.CurrentValue);
            base.Show();
        }

        public override void Hide()
        {
            LevelManager.CoreTemplete.HP.OnValueChanged -= UpdateUiData;
            base.Hide();
        }
    }
}