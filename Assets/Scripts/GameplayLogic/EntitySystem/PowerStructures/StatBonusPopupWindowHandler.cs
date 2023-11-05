using System;
using TMPro;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupWindowHandler : MonoBehaviour
    {
        public bool IsActive => _isActive;
        public int ActivePowerStructureId => _activePowerStructureId;
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private SpriteRenderer _popupVisual;
        private string _statBonusText;
        private bool _isActive;
        private int _activePowerStructureId = -1;

        public void ShowPopupWindow(int powerStructureId, string statBonusText, float value, Color color, float yAxisModifier)
        {
            _activePowerStructureId = powerStructureId;
            _popupVisual.gameObject.SetActive(true);
            _popupText.gameObject.SetActive(true);
            _isActive = true;
            _statBonusText = statBonusText;
            transform.localPosition = new Vector3(0,yAxisModifier,0);
            _popupText.text = $"{statBonusText} + {value}%";
            _popupVisual.color = color;
        }

        public void UpdatePopupWindow(float value, Color color)
        {
            _popupText.text = $"{_statBonusText} + {value}%";
            _popupVisual.color = color;
        }

        public void HidePopupWindow()
        {
            transform.position = Vector3.zero;
            _popupText.text = "";
            _popupVisual.gameObject.SetActive(false);
            _popupText.gameObject.SetActive(false);
            _isActive = false;
        }

    }
}