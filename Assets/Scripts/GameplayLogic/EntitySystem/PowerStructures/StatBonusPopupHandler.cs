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

        public void ShowPopupWindows(int PowerStructureId, int ringId, string statBonusText, float value)
        {
            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                if (_popupWindowHandlers[i].ActivePowerStructureId == PowerStructureId && _popupWindowHandlers[i].IsActive)
                {
                    //float alpha = _popupWindowStartingAlpha - _popupWindowAlphaChange * ringId;
                    _popupWindowHandlers[i].UpdatePopupWindow(ringId,value);
                    return;
                }
                if (_popupWindowHandlers[i].IsActive) continue;
                _popupWindowHandlers[i].ShowPopupWindow(PowerStructureId, ringId, statBonusText,value,(i)* StatEffectPopupWindowsDistance);
                return;
            }
        }

        public void HidePopupWindow(int powerStructureId)
        {
            foreach (var PopupWindowHandler in _popupWindowHandlers)
            {
                if (PopupWindowHandler.ActivePowerStructureId == powerStructureId)
                {
                    PopupWindowHandler.HidePopupWindow();
                }
            }
        }
    }
}