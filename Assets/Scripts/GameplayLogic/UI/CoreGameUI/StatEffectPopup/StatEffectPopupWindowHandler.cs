using System;
using TMPro;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatEffectPopupWindowHandler : MonoBehaviour
    {
        public bool IsActive => _isActive;
        public int ActiveEntityId => _activeEntityId;
        [SerializeField] private TextMeshPro _popupText;
        [SerializeField] private SpriteRenderer _popupVisual;
        private string _statBonusText;
        private bool _isActive;
        private int _activeEntityId= -1;

        private void OnValidate()
        {
            _popupText ??= GetComponentInChildren<TextMeshPro>();
            _popupVisual ??= GetComponentInChildren<SpriteRenderer>();
        }

        public void Init()
        {
            _popupVisual.gameObject.SetActive(false);
            _popupText.gameObject.SetActive(false);
        }
        public void ShowPopupWindow(int EntityId, string statBonusText, float value,bool isPercent, Color color, float yAxisModifier)
        {
            _activeEntityId = EntityId;
            _popupVisual.gameObject.SetActive(true);
            _popupText.gameObject.SetActive(true);
            _isActive = true;
            _statBonusText = statBonusText;
            transform.localPosition = new Vector3(0,yAxisModifier,0);
            _popupText.text = isPercent ? $"{_statBonusText} + {value}%" : $"{_statBonusText} + {value}";
            _popupVisual.color = color;
        }

        public void UpdatePopupWindow(float value,bool isPercent, Color color)
        {
            _popupText.text = isPercent ? $"{_statBonusText} + {value}%" : $"{_statBonusText} + {value}";
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