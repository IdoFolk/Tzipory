using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;

namespace Tzipory.GameplayLogic.UIElements
{
    public class CoreHPUIHnadler : BaseCounterUIHandler
    {

        public override void Init()
        {
            _maxCount.text = $"/{LevelManager.CoreTemplete.EntityHealthComponent.Health.BaseValue}";
            base.Init();
        }

        public override void Show()
        {

            LevelManager.CoreTemplete.EntityHealthComponent.Health.OnValueChanged += UpdateCoreUI;
            base.Show();
        }

        private void UpdateCoreUI(StatChangeData statChangeData)
        {
            UpdateUIVisual();
        }

        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            UpdateUiData(LevelManager.CoreTemplete.EntityHealthComponent.Health.CurrentValue);
        }

        public override void Hide()
        {
            LevelManager.CoreTemplete.EntityHealthComponent.Health.OnValueChanged -= UpdateCoreUI;
            base.Hide();
        }
    }
}