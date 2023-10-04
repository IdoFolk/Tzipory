using Tzipory.SerializeData.PlayerData.Party.Entity;
using UnityEngine;

namespace Tzipory.GamePlayLogic.UI.MetaUI
{
    public class CampUIManager : MonoBehaviour
    {
        [SerializeField] private CampFireUIHandler _campFireUIHandler;

        private void Awake()
        {
        }

        public void NewMainShamanSelected(ShamanDataContainer shamanDataContainer)
        {
            _campFireUIHandler.SetNewShamanData(shamanDataContainer);
        }
    }
}