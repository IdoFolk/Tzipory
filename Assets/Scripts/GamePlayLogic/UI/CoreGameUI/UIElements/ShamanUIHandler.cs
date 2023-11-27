using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class ShamanUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _splash;
        private Shaman _shaman;

        
        public void SetShamanData(Shaman shaman)
        {
            _shaman = shaman;
            _splash.sprite = _shaman.VisualConfig.Icon;
        }

        private void GoToShaman()=>
            GameManager.CameraHandler.SetCameraPosition(_shaman.transform.position);

        public override void Show()
        {
            _shaman.EntityHealthComponent.Health.OnValueChanged += OnHealthChange;
            OnClickEvent += GoToShaman;
            base.Show();
        }

        public override void Hide()
        {
            _shaman.EntityHealthComponent.Health.OnValueChanged -= OnHealthChange;
            OnClickEvent -= GoToShaman;
            base.Hide();
        }

        private void OnHealthChange(StatChangeData statChangeData)=>
            UpdateUIVisual();

        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            _healthBar.value  = _shaman.EntityHealthComponent.Health.CurrentValue / _shaman.EntityHealthComponent.Health.BaseValue;
            _fill.color = Color.Lerp(Color.red,Color.green,_shaman.EntityHealthComponent.Health.CurrentValue/_shaman.EntityHealthComponent.Health.BaseValue);
        }
    }
}