using System;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPlacementUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _splash;
        private bool _isActive;

        public event Action<PointerEventData> OnTotemClick;

        protected override void OnClick(PointerEventData eventData)
        {
            OnTotemClick?.Invoke(eventData);
        }

        private void Update()
        {
            if (_isActive)
            {
                transform.position = Input.mousePosition;
            }
        }

        public void ToggleSprite(bool state)
        {
            _splash.enabled = state;
            _isActive = state;
            if (!state) transform.position = Vector3.zero;
        }
    }
}