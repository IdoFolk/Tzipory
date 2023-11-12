using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class PartyUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _heroContainer;
        [SerializeField] private ShamanUIHandler _shamanUIHanlder;
        [SerializeField] private TotemPanelUIManager _totemPanelUIManager; //for instantiating totems in the ui pane


        public override void Init()
        {
            var shamans = LevelManager.PartyManager.Party;

            foreach (var shaman in shamans)
            {
                var shamanUI = Instantiate(_shamanUIHanlder, _heroContainer);
                shamanUI.SetShamanData(shaman);
                
                //if shaman has totem equipped then instantiate the totem in the totem panel ui
            }
            
            base.Init();
        }
    }
}