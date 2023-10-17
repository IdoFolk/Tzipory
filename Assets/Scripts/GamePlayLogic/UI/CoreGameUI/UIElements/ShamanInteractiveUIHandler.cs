﻿using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Scripts.Systems.PopupSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class ShamanInteractiveUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _splash;
        private Shaman _shaman;
        private PopupWindowHandler _popupWindowHandler;
        
        protected override void Awake()
        {
            UIManager.AddObserverObject(this);
        }
        
        public void Init(Shaman shaman)
        {
            _popupWindowHandler = GetComponentInParent<PartyUIManager>().PopupWindowHandler;
            _shaman = shaman;
            _splash.sprite = _shaman.SpriteRenderer.sprite;
            Show();
            UpdateUIData(_shaman.Health.CurrentValue);
        }

        private void GoToShaman()
        {
            GameManager.CameraHandler.SetCameraPosition(_shaman.transform.position);
        }

        private void OpenPopupWindow()
        {
            _popupWindowHandler.OpenWindow(_shaman.name,"very strong yes yes");
        }
        private void ClosePopupWindow()
        {
            _popupWindowHandler.CloseWindow();
        }

        public override void Show()
        {
            _shaman.Health.OnValueChangedData += OnHealthChange;
            OnClickEvent += GoToShaman;
            OnEnter += OpenPopupWindow;
            OnExit += ClosePopupWindow;
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

        private void UpdateUIData(float cureentHP)
        {
            _healthBar.value  = cureentHP / _shaman.Health.BaseValue;
            _fill.color = Color.Lerp(Color.red,Color.green,_shaman.Health.CurrentValue/_shaman.Health.BaseValue);
        }
    }
}