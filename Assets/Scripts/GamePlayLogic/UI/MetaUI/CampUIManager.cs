using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GamePlayLogic.UI.MetaUI
{
    public class CampUIManager : BaseUIElement
    {
        [SerializeField] private CampFireUIHandler _campFireUIHandler;
        
        public void NewMainShamanSelected(ShamanDataContainer shamanDataContainer)
        {
            _campFireUIHandler.SetNewShamanData(shamanDataContainer);
        }
    }
}