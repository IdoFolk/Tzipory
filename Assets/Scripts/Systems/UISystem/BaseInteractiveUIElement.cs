﻿using System;
using Sirenix.OdinInspector;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseInteractiveUIElement : BaseUIElement , IPointerEnterHandler,IPointerExitHandler , IPointerClickHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public event Action OnClickEvent;
        public event Action OnDragEvent;
        public event Action OnBeginDragEvent;
        public event Action OnEndDragEvent;
        public event Action OnDoubleClickEvent;
        public event Action OnEnter;
        public event Action OnExit;


        [SerializeField] private bool _enableDrag;
        
        [SerializeField,ShowIf("_enableDrag")] private CanvasGroup _canvasGroup;
        
        [SerializeField] private float _doubleClickSpeed = 0.5f;
        
        private bool _isOn;
        
        private int _clickNum;

        private ITimer _doubleClickTimer;
        
        private void Update()
        {
            if (_clickNum == 0)
                return;

            _doubleClickTimer ??= GAME_TIME.TimerHandler.StartNewTimer(_doubleClickSpeed,"Double Click UI Timer");
            
            if (_doubleClickTimer.IsDone)
            {
                _clickNum  = 0;
                _doubleClickTimer  = null;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _isOn = true;
            OnEnter?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _isOn = false;
            OnExit?.Invoke();
        }

        #region DragLogic

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!_enableDrag)
                return;
            
            _clickNum = 0;
            _doubleClickTimer = null;
            
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            OnBeginDragEvent?.Invoke();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(!_enableDrag)
                return;
            OnDragEvent?.Invoke();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!_enableDrag)
                return;
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            
            OnEndDragEvent?.Invoke();
        }

        #endregion
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_clickNum)
            {
                case 0:
                    OnClick(eventData);
                    return;
                case 1:
                    OnDoubleClick(eventData);
                    return;
            }
        }

        protected virtual void OnClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke();
            _clickNum++;
        }
        
        protected virtual void OnDoubleClick(PointerEventData eventData)
        {
            OnDoubleClickEvent?.Invoke();
            _doubleClickTimer = null;
            _clickNum = 0;
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
        }
    }
}