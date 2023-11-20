using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatEffectPopupHandler : MonoBehaviour
    {
        [SerializeField] private StatEffectPopupWindowHandler[] _popupWindowHandlers;
        [SerializeField] private float StatEffectPopupWindowsDistance;
        

        private void Awake()
        {
            foreach (var popupWindowHandler in _popupWindowHandlers)
            {
                popupWindowHandler.Init();
            }
        }

        public void ShowPopupWindows(int EntityId, string statBonusText, float value, bool isPercent, Color color)
        {
            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                if (_popupWindowHandlers[i].ActiveEntityId == EntityId && _popupWindowHandlers[i].IsActive)
                {
                    _popupWindowHandlers[i].UpdatePopupWindow(value, isPercent, color);
                    return;
                }
            }

            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                if (_popupWindowHandlers[i].IsActive) continue;
                _popupWindowHandlers[i].ShowPopupWindow(EntityId, statBonusText, value, isPercent, color, (i) * StatEffectPopupWindowsDistance);
                return;
            }
        }

        public void HidePopupWindow(int powerStructureId)
        {
            bool deleted = false;
            foreach (var popupWindowHandler in _popupWindowHandlers)
            {
                if (popupWindowHandler.ActiveEntityId == powerStructureId && popupWindowHandler.IsActive)
                {
                    popupWindowHandler.HidePopupWindow();
                    deleted = true;
                }
            }

            if (!deleted)
                Debug.LogError("Did not found the correct power structure id");
        }
    }
}