using System;
using Tools.Enums;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        [SerializeField] private bool _showOnAwake = false;
        
        public string ElementName { get; }
        public Action OnShow { get; }
        public Action OnHide { get; }

        public bool IsInitialization { get; private set; }
        protected abstract UIGroup UIGroup { get; }

        protected virtual void Awake()
        {
            UIManager.AddUIElement(UIGroup,this);
            
            if (_showOnAwake)
                Show();
            else
                gameObject.SetActive(false);
        }

        private void OnDestroy() =>
            UIManager.RemoveUIElement(this);

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
    }
}