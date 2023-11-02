using System;
using TMPro;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupWindowHandler : MonoBehaviour
    {
        public bool IsActive => _isActive;
        public int ActivePowerStructureId => _activePowerStructureId;
        public int ActiveRingId => _activeRingId;
        public string StatBonusText => _statBonusText;
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private SpriteRenderer _popupVisual; //for changing the color
        private string _statBonusText;
        private bool _isActive;
        private int _activePowerStructureId;
        private int _activeRingId;

        public void ShowPopupWindow(int powerStructureId, int ringId, string statBonusText, float value, float yAxisModifier)
        {
            _activePowerStructureId = powerStructureId;
            _activeRingId = ringId;
            _popupVisual.gameObject.SetActive(true);
            _popupText.gameObject.SetActive(true);
            _isActive = true;
            _statBonusText = statBonusText;
            transform.localPosition = new Vector3(0,yAxisModifier,0);
            _popupText.text = $"{statBonusText} + {value}%";
            // var color = _popupVisual.color;
            // color.a = alphaValue;
            // _popupVisual.color = color;
        }

        public void UpdatePopupWindow(int ringId, float value)
        {
            _activeRingId = ringId;
            _popupText.text = $"{_statBonusText} + {value}%";
            // var color = _popupVisual.color;
            // color.a = alphaValue;
            // _popupVisual.color = color;
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