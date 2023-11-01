using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
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

        public static void ShowPopupWindows(string statBonusText, float value)
        {
            _statBonusPopupHandler.ShowPopupWindows(statBonusText, value);
        }
    }
}