using System;
using TMPro;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _splash;
        [SerializeField] private TextMeshProUGUI _keybindText;
        private TotemConfig _totemConfig;
        public event Action<int> OnTotemClick;
        private int _totemId;
        private KeyCode _currentKeybind;

        public int ShamanId { get; private set; }

        private bool _totemPlaced;


        public void Init(TotemConfig totemConfig, int shamanId, KeyCode keybind, int keybindOrder)
        {
            _totemConfig = totemConfig;
            _splash.sprite = totemConfig.TotemSprite;
            _keybindText.text = $"{keybindOrder}";
            var fixedColor = totemConfig.RingColor;
            fixedColor.a = 1;
            _splash.color = fixedColor;
            ShamanId = shamanId;
            _currentKeybind = keybind;
            base.Init();
        }

        protected override void Update()
        {
            base.Update();

            if (!Input.GetKeyDown(_currentKeybind)) return;
            OnTotemClick?.Invoke(ShamanId);
        }

        protected override void OnClick(PointerEventData eventData)
        {
            if (_totemPlaced) return;
            OnTotemClick?.Invoke(ShamanId);
        }
        public void ShowTotemPlaced()
        {
            _totemPlaced = true;
            _splash.enabled = false;
        }
    }
}