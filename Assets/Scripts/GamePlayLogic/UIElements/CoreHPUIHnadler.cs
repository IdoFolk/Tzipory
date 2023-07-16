namespace GameplayeLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {
        private CoreTemple _coreTemple;

        private void Start()
        {
            _coreTemple = GameManager.CoreTemplete;
            Show();//temp
        }

        public override void Show()
        {
            _coreTemple.HP.OnValueChanged += UpdateUiData;
            _maxCount.text = $"/{_coreTemple.HP.BaseValue}";
            UpdateUiData(_coreTemple.HP.CurrentValue);
            base.Show();
        }

        public override void Hide()
        {
            _coreTemple.HP.OnValueChanged -= UpdateUiData;
            base.Hide();
        }
    }
}