namespace GameplayeLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {
        public override void Show()
        {
            GameManager.CoreTemplete.HP.OnValueChanged += UpdateUiData;
            _maxCount.text = $"/{GameManager.CoreTemplete.HP.BaseValue}";
            UpdateUiData(GameManager.CoreTemplete.HP.CurrentValue);
            base.Show();
        }

        public override void Hide()
        {
            GameManager.CoreTemplete.HP.OnValueChanged -= UpdateUiData;
            base.Hide();
        }
    }
}