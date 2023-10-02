using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class ShamanUiHandler : BaseUIElement
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _splash;
        private Shaman _shaman;
        
        public void Init(Shaman shaman)
        {
            _shaman = shaman;
            _splash.sprite = _shaman.SpriteRenderer.sprite;
            Show();
            UpdateUIData(_shaman.Health.CurrentValue);
        }

        private void GoToShaman()
        {
            GameManager.CameraHandler.SetCameraPosition(_shaman.transform.position);
        }

        public override void Show()
        {
            _shaman.Health.OnValueChangedData += OnHealthChange;
            OnDoubleClick += GoToShaman;
            base.Show();
        }

        public override void Hide()
        {
            _shaman.Health.OnValueChangedData -= OnHealthChange;
            OnDoubleClick -= GoToShaman;
            base.Hide();
        }

        private void OnHealthChange(StatChangeData statChangeData)
        {
            UpdateUIData(statChangeData.NewValue);
        }

        private void UpdateUIData(float cureentHP)
        {
            _healthBar.value  = cureentHP / _shaman.Health.BaseValue;
            _fill.color = Color.Lerp(Color.red,Color.green,_shaman.Health.CurrentValue/_shaman.Health.BaseValue);
        }
    }
}