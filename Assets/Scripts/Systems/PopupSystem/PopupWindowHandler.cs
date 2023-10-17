using UnityEngine;
using TMPro;
using Tzipory.Systems.UISystem;

namespace Tzipory.Scripts.Systems.PopupSystem
{
    public class PopupWindowHandler : BaseUIElement
    {
        [SerializeField] private GameObject _uiHolder;
        [SerializeField] private TextMeshProUGUI _textHeader;
        [SerializeField] private TextMeshProUGUI _textbody;
        [SerializeField] private RectTransform _boxWindow;

        private Vector3 _fixedPosition;
        protected override void Awake()
        {
            
        }

        private void Start()
        {
            if (_uiHolder.activeSelf) _uiHolder.SetActive(false);
            
        }

        private void Update()
        {
            if (_uiHolder.activeSelf )
            {
                var rect = _boxWindow.rect;
                var mousePos = Input.mousePosition;
                var fixedVector = new Vector3(-rect.width, rect.height,0);
                _fixedPosition = mousePos + fixedVector;
                if (transform.position != mousePos - fixedVector)
                    transform.position = _fixedPosition;
            }
        }

        public void OpenWindow(string header,string body)
        {
            _textHeader.text = header;
            _textbody.text = body;
            _uiHolder.SetActive(true);
        }

        public void CloseWindow()
        {
            _uiHolder.SetActive(false);
        }
    }
}