using System.Collections;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupHandler : MonoBehaviour
    {
        [SerializeField] private StatBonusPopupWindowHandler[] _popupWindowHandlers;
        [SerializeField] private float StatEffectPopupWindowsDistance;
        private List<string> _statBonusTexts;

        
        public void ShowPopupWindows(string statBonusText, float value)
        {
            _statBonusTexts.Add(statBonusText);
            for (int i = 0; i < _popupWindowHandlers.Length; i++)
            {
                if (_popupWindowHandlers[i].IsActive) continue;
                _popupWindowHandlers[i].ShowPopupWindow(_statBonusTexts[i],value,(i)* StatEffectPopupWindowsDistance);
                return;
            }
        }

        public void HidePopupWindow(string statBonusText)
        {
            foreach (var statBonusPopup in _popupWindowHandlers)
            {
                if (statBonusPopup.PopupText.text == statBonusText)
                {
                    statBonusPopup.HidePopupWindow();
                }
            }
        }
    }
}