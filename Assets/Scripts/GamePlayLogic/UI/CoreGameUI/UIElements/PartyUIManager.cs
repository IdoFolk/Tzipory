using Tools.Enums;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class PartyUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _heroContainer;
        [SerializeField] private ShamanUIHandler _shamanUIHanlder;

        protected override UIGroup UIGroup => UIGroup.GameUI;

        public override void Init()
        {
            var shamans = LevelManager.PartyManager.Party;

            foreach (var shaman in shamans)
            {
                var shamanUI = Instantiate(_shamanUIHanlder, _heroContainer);
                shamanUI.SetShamanData(shaman);
            }
            
            base.Init();
        }
    }
}