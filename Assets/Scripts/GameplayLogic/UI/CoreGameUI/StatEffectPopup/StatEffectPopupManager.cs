using Tzipory.GameplayLogic.UIElements;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatEffectPopupManager : MonoBehaviour
    {

        [SerializeField] private Transform _parentHolder;
        [SerializeField] private GameObject _statEffectPopupHandlerPrefab;

        private static StatEffectPopupHandler _statEffectPopupHandler;

        private void Awake()
        {
            _statEffectPopupHandler = Instantiate(_statEffectPopupHandlerPrefab, _parentHolder).GetComponent<StatEffectPopupHandler>();
            
        }

        public static void ShowPopupWindows(int EntityId, string statBonusText, float value, bool isPercent, Color color)
        {
            if (TotemPanelUIManager.TotemSelected) return;
            _statEffectPopupHandler.ShowPopupWindows(EntityId, statBonusText, value, isPercent, color);
        }
        public static void HidePopupWindows(int powerStructureId)
        {
            if (TotemPanelUIManager.TotemSelected) return;
            _statEffectPopupHandler.HidePopupWindow(powerStructureId);
        }
    }
}