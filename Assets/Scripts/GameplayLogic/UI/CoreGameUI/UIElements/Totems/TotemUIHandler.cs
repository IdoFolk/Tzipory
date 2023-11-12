using System;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] private Image _splash;
        private Totem _totem;
        public event Action<int> OnTotemClick;
        private int _id;
        private bool _cooldownActive;
        private float _cooldownTimer;

        private Color _cooldownColor;
        private Color _activeColor;


        public void Init(int id)
        {
            _id = id;
            base.Init();
            _activeColor = _splash.color;
            _cooldownColor = _activeColor;
            _cooldownColor.a = 0.5f;
        }

        private void Update()
        {
            if (_cooldownActive)
            {
                _cooldownTimer -= GAME_TIME.GameDeltaTime;
                if (_cooldownTimer <= 0)
                {
                    _cooldownActive = false;
                    _cooldownTimer = _totem.TotemConfig.TotemCooldown;
                    _splash.color = _activeColor;
                }
            }
        }

        protected override void OnClick(PointerEventData eventData)
        {
            if (_cooldownActive) return;
            base.OnClick(eventData);
            OnTotemClick?.Invoke(_id);
        }

        public void SetTotemData(Totem totem)
        {
            _totem = totem;
            _splash.sprite = _totem.TotemConfig.TotemSprite;
        }

        public void ActivateCooldown()
        {
            _cooldownActive = true;
            _splash.color = _cooldownColor;
        }
    }
}