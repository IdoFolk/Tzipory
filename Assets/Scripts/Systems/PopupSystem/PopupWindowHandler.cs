using UnityEngine;
using TMPro;
using Tzipory.Systems.UISystem;

namespace Tzipory.Systems.PopupSystem
{
    public class PopupWindowHandler : BaseUIElement
    {
        [SerializeField] private GameObject _uiHolder;
        [SerializeField] private TextMeshProUGUI _textHeader;
        [SerializeField] private TextMeshProUGUI _textbody;
        [SerializeField] private Vector2 _defaultPadding;
        private RectTransform _rectTransform;

        private Vector3 _fixedPosition;
        protected override void Awake()
        {
            
        }

        private void Start()
        {
            if (_rectTransform is null) _rectTransform = GetComponent<RectTransform>();
            if (_uiHolder.activeSelf) _uiHolder.SetActive(false);
        }

        public void OpenWindow(RectTransform rectTransform,string header,string body)
        {
            _textHeader.text = header;
            _textbody.text = body;
            var uiElementUpperRightCorner = new Vector3((rectTransform.rect.width / 2) + _defaultPadding.x, (rectTransform.rect.height / 2) + _defaultPadding.y, 0);
            Vector3 uiElementPopupPosition = rectTransform.position + uiElementUpperRightCorner;
            var windowPopupPosition = new Vector3(_rectTransform.rect.width/2, _rectTransform.rect.height/2,0);
            _fixedPosition = uiElementPopupPosition + windowPopupPosition;
            transform.position = _fixedPosition;
            _uiHolder.SetActive(true);
        }

        public void CloseWindow()
        {
            _uiHolder.SetActive(false);
        }

        private void CheckIfWindowFitsScreen()
        {
            
        }
    }
}