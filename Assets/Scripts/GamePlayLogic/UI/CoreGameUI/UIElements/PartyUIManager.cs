using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class PartyUIManager : BaseInteractiveUIElement
    {
        [SerializeField] private RectTransform _heroContainer;
        [SerializeField] private ShamanInteractiveUIHandler _shamanInteractiveUIHanlder;
        
        protected override void Awake()
        {
            UIManager.AddObserverObject(this);
        }
        
        public override void Show()
        {
            var shamans = LevelManager.PartyManager.Party;

            foreach (var shaman in shamans)
            {
                var shamanUI = Instantiate(_shamanInteractiveUIHanlder, _heroContainer);
                shamanUI.Init(shaman);
            }
            
            base.Show();
        }
    }
}