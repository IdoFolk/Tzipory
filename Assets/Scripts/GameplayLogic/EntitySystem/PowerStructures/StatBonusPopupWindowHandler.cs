using System;
using TMPro;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupWindowHandler : MonoBehaviour
    {
        public bool IsActive => _isActive;
        public TextMeshPro PopupText => _popupText;
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private SpriteRenderer _popupVisual; //for changing the color
        private bool _isActive;

        public void ShowPopupWindow(string statBonusText, float value, float yAxisModifier)
        {
            var position = transform.position;
            position = new Vector3(position.x,position.y + yAxisModifier, position.z);
            transform.position = position;
            _popupText.text = $"{statBonusText} + {value}%";
            gameObject.SetActive(true);
            _isActive = true;
        }

        public void HidePopupWindow()
        {
            gameObject.SetActive(false);
            _isActive = false;
        }

    }
}