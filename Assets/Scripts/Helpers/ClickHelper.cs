using System;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.Helpers
{
    public class ClickHelper : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnClick;
        public event Action OnHoldClickStart;
        public event Action OnHoldClickFinish;
        public event Action OnEnterHover;
        public event Action OnExitHover;

        public bool IsHover { get; private set; }

        private const int CLICKABLE_LAYER_INDEX = 11;
        private const float HOLD_CLICK_WAIT_TIME = 1.5f;

        private float _holdClickWaitTime;
        private float _holdClickTimer;
        private bool _holdClickTimerActive;

        public bool HoldClickTimerActive => _holdClickTimerActive;

        public float HoldClickWaitTime => _holdClickWaitTime;

        public float HoldClickTimer => _holdClickTimer;


        private void Awake()
        {
            _holdClickWaitTime = HOLD_CLICK_WAIT_TIME;
            gameObject.layer = CLICKABLE_LAYER_INDEX;
            _holdClickTimer = _holdClickWaitTime;
        }


        public void SetHoldClickWaitTime(float waitTime)
        {
            if (waitTime == 0)
                _holdClickTimer = HOLD_CLICK_WAIT_TIME;
            else
                _holdClickWaitTime = waitTime;

            _holdClickTimer = _holdClickWaitTime;
        }

        private void Update()
        {
            if (!_holdClickTimerActive) return;

            if (_holdClickTimer <= 0)
            {
                _holdClickTimer = HOLD_CLICK_WAIT_TIME;
                _holdClickTimerActive = false;
                OnHoldClickFinish?.Invoke();
            }

            _holdClickTimer -= GAME_TIME.GameDeltaTime;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHover = true;
            OnEnterHover?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHover = false;
            OnExitHover?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _holdClickTimerActive = true;
            OnHoldClickStart?.Invoke();
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _holdClickTimerActive = false;
            _holdClickTimer = HOLD_CLICK_WAIT_TIME;
            Cursor.visible = true;
        }
    }
}