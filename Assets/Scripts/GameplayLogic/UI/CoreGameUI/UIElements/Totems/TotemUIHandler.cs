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
        public event Action<int,int> OnTotemClick;
        private int _totemId;
        private int _shamanId;

        public int ShamanId => _shamanId;

        private bool _totemPlaced;
        private Color _disabledColor;
        private Color _activeColor;


        public void Init(TotemConfig totemConfig, int shamanId)
        {
            _totemId = totemConfig.ObjectId;
            _shamanId = shamanId;
            _splash.sprite = totemConfig.TotemSprite;
            _activeColor = _splash.color;
            _disabledColor = _activeColor;
            _disabledColor.a = 0.5f;
            base.Init();
        }

        protected override void OnClick(PointerEventData eventData)
        {
            if (_totemPlaced) return;
            base.OnClick(eventData);
            OnTotemClick?.Invoke(_totemId,_shamanId);
        }

        public void SetTotemData(TotemConfig totemConfig)
        {
            _totemConfig = totemConfig;
            _splash.sprite = totemConfig.TotemSprite;
        }

        public void ShowTotemPlaced()
        {
            _totemPlaced = true;
            _splash.color = _disabledColor;
        }
    }
}