using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPanelUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _totemContainer;
        [SerializeField] private TotemUIHandler _totemUIHandler;
        [SerializeField] private TotemPlacementUIHandler _totemPlacementUIHandler;
        
        [SerializeField] private List<TotemUIHandler> _totemUIHandlers; //temp
        private bool _placementActive;
        private int _activeShamanId;
        public override void Init()
        {
            _totemPlacementUIHandler.OnTotemClick += OnTotemPlacementClick;
            LevelManager.TotemManager.TotemPlaced += HideTotemUI;
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
            _totemPlacementUIHandler.OnTotemClick -= OnTotemPlacementClick;
            LevelManager.TotemManager.TotemPlaced -= HideTotemUI;
            foreach (var totemUIHandler in _totemUIHandlers)
            {
                totemUIHandler.OnTotemClick -= OnTotemClick;
            }
            base.Hide();
        }
        private void OnTotemPlacementClick(PointerEventData eventData)
        {
            if (!_placementActive) return;
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                {
                    _totemPlacementUIHandler.ToggleSprite(false);
                    _placementActive = false;
                    LevelManager.TotemManager.PlaceTotem(_activeShamanId);
                    break;
                }
                case PointerEventData.InputButton.Right:
                    _totemPlacementUIHandler.ToggleSprite(false);
                    _placementActive = false;
                    break;
            }
        }

        private void OnTotemClick(int totemId, int shamanId) //totemId may be redundent
        {
            // foreach (var shaman in LevelManager.PartyManager.Party.Where(shaman => shaman.EntityInstanceID == _activeShamanId))
            // {
            //     shaman.TempHeroMovement.SelectHero();
            // }
            _totemPlacementUIHandler.ToggleSprite(true);
            _placementActive = true;
            _activeShamanId = shamanId;
        }

        public void HideTotemUI()
        {
            foreach (var totemUI in _totemUIHandlers.Where(totemUI => totemUI.ShamanId == _activeShamanId))
            {
                totemUI.ShowTotemPlaced();
            }
        }
    }
}