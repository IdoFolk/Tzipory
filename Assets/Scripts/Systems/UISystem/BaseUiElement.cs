﻿using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public Action OnShow;
        public Action OnHide;
        
        [SerializeField] private bool _showOnAwake = false;
        [SerializeField] private UIGroup _uiGroupTags;
        [SerializeField,HideInInspector]protected internal RectTransform _rectTransform;
        
        public string ElementName => gameObject.name;
        public UIGroup UIGroupTags => _uiGroupTags;

        public bool IsInitialization { get; protected set; }

        public Vector2 UIScreenPoint => GameManager.CameraHandler.MainCamera.WorldToScreenPoint(transform.position);

        protected virtual void Awake()
        {
            UIManager.AddUIElement(this,UIGroupTags);
            
            if (_showOnAwake)
                Show();
            else
                gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            UIManager.RemoveUIElement(this);
            Hide();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        public virtual void UpdateUIVisual()
        {
            
        }

        public virtual void Init()
        {
            IsInitialization = true;
        }

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }
    }
}