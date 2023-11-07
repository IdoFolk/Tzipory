using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        [SerializeField] private bool _showOnAwake = false;
        [SerializeField] private UIGroup _uiGroupTags;
        
        protected RectTransform _rectTransform;
        
        public string ElementName => gameObject.name;
        public Action OnShow { get; }
        public Action OnHide { get; }
        public UIGroup UIGroupTags => _uiGroupTags;

        public bool IsInitialization { get; private set; }

        protected virtual void Awake()
        {
            UIManager.AddUIElement(this,UIGroupTags);
            
            if (_showOnAwake)
                Show();
            else
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Hide();
            UIManager.RemoveUIElement(this);
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