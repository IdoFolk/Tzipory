using System;
using Systems.UISystem;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class PartyUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _heroContainer;
        [SerializeField] private ShamanUiHandler _shamanUiHanlder;

        private void Start()
        {
            Show();
        }

        public override void Show()
        {
            var shamans = GameManager.PartyManager.Party;

            foreach (var shaman in shamans)
            {
                var shamanUI = Instantiate(_shamanUiHanlder, _heroContainer);
                shamanUI.Init(shaman);
            }
            
            base.Show();
        }
    }
}