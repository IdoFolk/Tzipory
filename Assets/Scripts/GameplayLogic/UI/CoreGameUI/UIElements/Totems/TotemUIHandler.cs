using System;
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
        private TotemConfig _totemConfig;
        public event Action<int> OnTotemClick;
        private int _totemId;
        private int _shamanId;

        public int ShamanId => _shamanId;

        private bool _totemPlaced;


        public void Init(TotemConfig totemConfig, int shamanId)
        {
            _totemConfig = totemConfig;
            _splash.sprite = totemConfig.TotemSprite;
            var fixedColor = totemConfig.RingColor;
            fixedColor.a = 1;
            _splash.color = fixedColor;
            _shamanId = shamanId;
            base.Init();
        }

        protected override void OnClick(PointerEventData eventData)
        {
            if (_totemPlaced) return;
            OnTotemClick?.Invoke(_shamanId);
        }
        public void ShowTotemPlaced()
        {
            _totemPlaced = true;
            _splash.enabled = false;
        }
    }
}