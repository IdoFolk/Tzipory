using Sirenix.Utilities;
using UnityEngine;
using TMPro;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.UISystem;

namespace Tzipory.Systems.PopupSystem
{
    public class PopupWindowHandler : BaseUIElement
    {
        [SerializeField] private GameObject _uiHolder;
        [SerializeField] private TextMeshProUGUI _textHeader;
        [SerializeField] private TextMeshProUGUI _textBody;
        [SerializeField] private PopupWindowConfig _defaultPopupWindowConfig;
        private void Start()
        {
            if (_uiHolder.activeSelf) _uiHolder.SetActive(false);
        }
        
        public void OpenWindow(BaseInteractiveUIElement uiElement,string header,string body)
        {
            PopupWindowConfig uiElementPopupConfig;
            if (uiElement.PopupWindowConfig is null)
            {
                Debug.Log("PopupWindowSetting is Null");
                uiElementPopupConfig = _defaultPopupWindowConfig;
            }
            else
            {
                uiElementPopupConfig = uiElement.PopupWindowConfig;
            }
            Init(uiElementPopupConfig, header, body);
            transform.position = CalculateWindowPosition(uiElement._rectTransform, uiElementPopupConfig);
            _uiHolder.SetActive(true);
        }

        public void CloseWindow()
        {
            _uiHolder.SetActive(false);
        }

        private void Init(PopupWindowConfig popupWindowConfig, string header, string body)
        {
            _textHeader.fontSize = popupWindowConfig.HeaderFontSize;
            _textBody.fontSize = popupWindowConfig.BodyFontSize;
            _textHeader.text = header;
            _textBody.text = body;
            _rectTransform.rect.SetSize(popupWindowConfig.WindowSize.x, popupWindowConfig.WindowSize.y);
        }
        private Vector3 CalculateWindowPosition(RectTransform uiElementRect,PopupWindowConfig uiElementPopupConfig)
        {
            Vector3 padding = new Vector3(uiElementPopupConfig.PositionPadding.x,uiElementPopupConfig.PositionPadding.y,0);
            var PopupWindowPosition = uiElementRect.position + padding;
            var paddingCorrection = CheckIfWindowFitsScreen(PopupWindowPosition,uiElementRect);
            padding = Vector3.Scale(padding, paddingCorrection);
            PopupWindowPosition = uiElementRect.position + padding;
            return PopupWindowPosition;
        }
        private Vector3 CheckIfWindowFitsScreen(Vector3 PopupWindowPosition, RectTransform uiElementRect)
        {
            Vector3 paddingCorrection = new Vector3(1,1,0);
            var upperEdge = PopupWindowPosition + Vector3.up * (uiElementRect.rect.height/2); 
            var lowerEdge= PopupWindowPosition + Vector3.down * (uiElementRect.rect.height/2); 
            var rightEdge= PopupWindowPosition + Vector3.right * (uiElementRect.rect.width/2); 
            var leftEdge= PopupWindowPosition + Vector3.left * (uiElementRect.rect.width/2);
            
            if (upperEdge.y > GameManager.CameraHandler.MainCamera.pixelHeight || lowerEdge.y < 0) paddingCorrection.y = -1;
            if (rightEdge.x > GameManager.CameraHandler.MainCamera.pixelWidth || leftEdge.x < 0) paddingCorrection.x = -1;

            return paddingCorrection;
        }
    }
}