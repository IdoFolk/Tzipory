using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public string ElementName { get; }
        public Action OnShow { get; }
        public Action OnHide { get; }
        
        private void Awake()
        {
            UIManager.AddObserverObject(this);
        }

        private void OnDestroy()
        {
            UIManager.RemoveObserverObject(this);
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
    }
}