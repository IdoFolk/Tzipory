using System;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.Helpers
{
    public class ClickHelper : MonoBehaviour , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler, IPointerDownHandler,IPointerUpHandler
    {
        public event Action OnClick;
        public event Action OnHoldClick;
        public event Action OnEnterHover;
        public event Action OnExitHover;

        public bool IsHover { get; private set; }   
        
        private const int CLICKABLE_LAYER_INDEX = 11;

        private float _holdClickWaitTime = 2f;
        private float _holdClickTimer;
        private bool _holdClickTimerActive;
        private bool _holdClickTimerFinished;
        
        private void Awake()
        {
            gameObject.layer = CLICKABLE_LAYER_INDEX;
            _holdClickTimer = _holdClickWaitTime;
        }

        public void SetHoldClickWaitTime(float waitTime)
        {
            _holdClickWaitTime = waitTime;
            _holdClickTimer = _holdClickWaitTime;
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
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_holdClickTimerFinished) return;
            _holdClickTimerActive = false;
            _holdClickTimer = _holdClickWaitTime;
        }

        private void Update()
        {
            if (!_holdClickTimerActive) return;
            
            if (_holdClickTimer <= 0)
            {
                _holdClickTimerFinished = true;
                _holdClickTimer = _holdClickWaitTime;
                _holdClickTimerActive = false;
                OnHoldClick?.Invoke();
            }
            _holdClickTimer -= GAME_TIME.GameDeltaTime;
        }
    }
   
}