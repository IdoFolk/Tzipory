using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.PopupSystem;
using UnityEngine;

namespace Tzipory.Systems.PopupSystem
{
    public class PopupWindowManager : MonoBehaviour
    {
        [SerializeField] private PopupWindowHandler _popupWindowAsset;
        [SerializeField] private Transform _canvasParent;
        private static PopupWindowHandler _popupWindow;

        private void Awake()
        {
            _popupWindow = Instantiate(_popupWindowAsset,_canvasParent);
        }

        public static void OpenNewWindow(RectTransform rectTransform,string header,string body)
        {
            _popupWindow.OpenWindow(rectTransform,header,body);
        }
        public static void OpenNewWindow(Transform transform,string header,string body) //may need to request sprite instead of transform
        {
            
        }
        public static void CloseNewWindow()
        {
            _popupWindow.CloseWindow();
        }
    }
}