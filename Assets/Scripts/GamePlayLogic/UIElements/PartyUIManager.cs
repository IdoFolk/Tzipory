using Systems.UISystem;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class PartyUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _heroContainer;
        [SerializeField] private ShamanUiHandler _shamanUiHanlder;
        
        public override void Show()
        {
            var shamans = LevelManager.PartyManager.Party;

            foreach (var shaman in shamans)
            {
                var shamanUI = Instantiate(_shamanUiHanlder, _heroContainer);
                shamanUI.Init(shaman);
            }
            
            base.Show();
        }
    }
}