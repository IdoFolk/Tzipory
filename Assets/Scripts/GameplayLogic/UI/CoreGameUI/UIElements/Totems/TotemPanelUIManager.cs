using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPanelUIManager : BaseUIElement
    {
        public static bool TotemSelected { get; private set; }
        [SerializeField] private RectTransform _totemContainer;
        [SerializeField] private TotemUIHandler _totemUIHandler;
        
        [SerializeField] private List<TotemUIHandler> _totemUIHandlers; //temp
        private bool _placementActive;
        private int _activeShamanId;
        public override void Init()
        {
            TotemManager.TotemPlaced += HideTotemUI;
            foreach (var shaman in LevelManager.PartyManager.Party)
            {
                if (shaman.TotemConfig is null) return;
                var totemUI = Instantiate(_totemUIHandler, _totemContainer);
                totemUI.Init(shaman.TotemConfig,shaman.EntityInstanceID);
                _totemUIHandlers.Add(totemUI);
                totemUI.OnTotemClick += OnTotemClick;
            }            
            base.Init();
        }

        public override void Hide()
        {
            TotemManager.TotemPlaced -= HideTotemUI;
            foreach (var totemUIHandler in _totemUIHandlers)
            {
                totemUIHandler.OnTotemClick -= OnTotemClick;
            }
            base.Hide();
        }

        private void OnTotemClick(int totemId, int shamanId) //totemId may be redundent
        {
            foreach (var shaman in LevelManager.PartyManager.Party.Where(shaman => shaman.EntityInstanceID == shamanId))
            {
                TotemSelected = true;
                shaman.TempHeroMovement.SelectHero();
            }
            _activeShamanId = shamanId;
        }

        public void HideTotemUI()
        {
            foreach (var totemUI in _totemUIHandlers.Where(totemUI => totemUI.ShamanId == _activeShamanId))
            {
                totemUI.ShowTotemPlaced();
            }
        }

        public static void ToggleTotemSelected(bool state)
        {
            TotemSelected = state;
        }
    }
}