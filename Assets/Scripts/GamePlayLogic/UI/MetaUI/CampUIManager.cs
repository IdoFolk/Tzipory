using System;
using UnityEngine;

namespace Tzipory.GamePlayLogic.UI.MetaUI
{
    public class CampUIManager : MonoBehaviour
    {
        [SerializeField] private CampFireUIHandler _campFireUIHandler;
        [SerializeField] private CharacterRosterUI _characterRosterUI;

        private void Awake()
        {
        }
    }
}