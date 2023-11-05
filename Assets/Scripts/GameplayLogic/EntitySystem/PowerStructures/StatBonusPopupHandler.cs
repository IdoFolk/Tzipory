using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupHandler : MonoBehaviour
    {
        [SerializeField] private StatBonusPopupWindowHandler[] _popupWindowHandlers;
        [SerializeField] private float StatEffectPopupWindowsDistance;
        [SerializeField,Range(0,1)] private float _popupWindowStartingAlpha;
        [SerializeField,Range(0,1)] private float _popupWindowAlphaChange;


        public void ShowPopupWindows(int PowerStructureId, int ringId, string statBonusText, float value, Color color)
        {
            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                float alpha = _popupWindowStartingAlpha - _popupWindowAlphaChange * ringId;
                color.a = alpha;
                if (_popupWindowHandlers[i].ActivePowerStructureId == PowerStructureId && _popupWindowHandlers[i].IsActive)
                {
                    _popupWindowHandlers[i].UpdatePopupWindow(value,color);
                    return;
                }
            }
            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                if (_popupWindowHandlers[i].IsActive) continue;
                _popupWindowHandlers[i].ShowPopupWindow(PowerStructureId, statBonusText,value,color,(i)* StatEffectPopupWindowsDistance);
                return;
            }
        }
  
        public void HidePopupWindow(int powerStructureId)
        {
            bool deleted = false;
            foreach (var popupWindowHandler in _popupWindowHandlers)
            {
                if (popupWindowHandler.ActivePowerStructureId == powerStructureId && popupWindowHandler.IsActive)
                {
                    popupWindowHandler.HidePopupWindow();
                    deleted = true;
                }
            }
            if(!deleted)
                Debug.LogError("Did not found the correct power structure id");
        }
    }
}