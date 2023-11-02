using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class StatBonusPopupManager : MonoBehaviour
    {

        [SerializeField] private Transform _parentHolder;
        [SerializeField] private GameObject _statBonusPopHandlerPrefab;

        private static StatBonusPopupHandler _statBonusPopupHandler;

        private void Awake()
        {
            _statBonusPopupHandler = Instantiate(_statBonusPopHandlerPrefab, _parentHolder).GetComponent<StatBonusPopupHandler>();
        }

        public static void ShowPopupWindows(int powerStructureId, int ringId, string statBonusText, float value)
        {
            _statBonusPopupHandler.ShowPopupWindows(powerStructureId, ringId, statBonusText, value);
        }
        public static void HidePopupWindows(int powerStructureId)
        {
            _statBonusPopupHandler.HidePopupWindow(powerStructureId);
        }
    }
}