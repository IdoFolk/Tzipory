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

        protected abstract UIGroupType GroupIndex { get; }

        protected virtual void Awake()
        {
            UIManager.AddObserverObject(this,(int)GroupIndex);
            
            if (_showOnAwake)
                Show();
        }

        private void OnDestroy() =>
            UIManager.RemoveObserverObject(this);

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