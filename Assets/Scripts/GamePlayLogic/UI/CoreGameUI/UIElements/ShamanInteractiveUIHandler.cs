using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.PopupSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class ShamanInteractiveUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _splash;
        private Shaman _shaman;
        
        protected override void Awake()
        {
            UIManager.AddObserverObject(this);
        }
        
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
            OnClickEvent += GoToShaman;
            base.Show();
        }

        public override void Hide()
        {
            _shaman.Health.OnValueChangedData -= OnHealthChange;
            OnClickEvent -= GoToShaman;
            base.Hide();
        }

        private void OnHealthChange(StatChangeData statChangeData)
        {
            UpdateUIData(statChangeData.NewValue);
        }

        private void UpdateUIData(float currentHP)
        {
            _healthBar.value  = currentHP / _shaman.Health.BaseValue;
            _fill.color = Color.Lerp(Color.red,Color.green,_shaman.Health.CurrentValue/_shaman.Health.BaseValue);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            var shamanName = _shaman.name;
            int index = shamanName.IndexOf('_');
            shamanName = shamanName.Substring(0, index);
            PopupWindowManager.OpenNewWindow(_rectTransform,shamanName,"Stats");
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            PopupWindowManager.CloseNewWindow();
        }
    }
}