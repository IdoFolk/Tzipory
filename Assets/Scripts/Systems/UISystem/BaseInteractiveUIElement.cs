using System;
using GamePlayLogic.Managers;
using Tzipory.BaseSystem.TimeSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace  Systems.UISystem
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
    
    public abstract class BaseInteractiveUIElement : MonoBehaviour , IUIElement , IPointerEnterHandler,IPointerExitHandler , IPointerClickHandler
    {
        public event Action OnClick;
        public event Action OnDoubleClick;
        public event Action OnEnter;
        public event Action OnExit;
        public Action OnShow { get; }

        public Action OnHide { get; }
        public string ElementName => gameObject.name;
        
        [SerializeField] private float _doubleClickSpeed = 0.5f;
        
        private bool _isOn;
        
        private int _clickNum;

        private ITimer _doubleClickTimer;

        private void Awake()
        {
            UIManager.AddObserverObject(this);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
            _clickNum = 0; 
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

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

        public void OnPointerEnter(PointerEventData eventData)=>
            PointerEnter(eventData);

        public void OnPointerExit(PointerEventData eventData) => 
            PointerExit(eventData);
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_clickNum)
            {
                case 0:
                    Click(eventData);
                    return;
                case 1:
                   DoubleClick(eventData);
                    return;
            }
        }

        protected virtual void Click(PointerEventData eventData)
        {
            OnClick?.Invoke();
            _clickNum++;
        }
        
        protected virtual void DoubleClick(PointerEventData eventData)
        {
            OnDoubleClick?.Invoke();
            _doubleClickTimer = null;
            _clickNum = 0;
        }

        protected virtual void PointerEnter(PointerEventData eventData)
        {
            _isOn = true;
            OnEnter?.Invoke();
        }
        
        protected virtual void PointerExit(PointerEventData eventData)
        {
            _isOn = false;
            OnExit?.Invoke();
        }
    }

    public interface IUIElement
    {
        public string ElementName { get; }
        public Action OnShow { get; }
        public Action OnHide { get; }
        
        void Show();
        void Hide();
    }
}