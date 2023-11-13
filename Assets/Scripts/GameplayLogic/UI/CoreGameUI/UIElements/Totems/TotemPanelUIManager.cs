using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.UISystem;
using Unity.VisualScripting;
using UnityEngine;

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
            foreach (var shaman in LevelManager.PartyManager.Party)
            {
                if (shaman.Totem is null) return;
                var totemUI = Instantiate(_totemUIHandler, _totemContainer);
                totemUI.SetTotemData(shaman.Totem);
                totemUI.Init(shaman.Totem,shaman.EntityInstanceID);
                _totemUIHandlers.Add(totemUI);
                totemUI.OnTotemClick += OnTotemClick;
            }            
            base.Init();
        }
        //add unsubscribe to event
        public override void Hide()
        {
            foreach (var totemUIHandler in _totemUIHandlers)
            {
                totemUIHandler.OnTotemClick -= OnTotemClick;
            }
            base.Hide();
        }

        private void Update() // temp
        {
            if (_placementActive)
            {
                if(Input.GetMouseButtonDown(0))
                {
                   _totemPlacementUIHandler.ToggleSprite(false);
                   _placementActive = false;
                   PlaceTotem();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _totemPlacementUIHandler.ToggleSprite(false);
                    _placementActive = false;
                }
            }
        }

        private void OnTotemClick(int totemId, int shamanId) //totemId may be redundent
        {
            _totemPlacementUIHandler.ToggleSprite(true);
            _placementActive = true;
            _activeShamanId = shamanId;
        }

        private void PlaceTotem()
        {
            TotemsManager.PlaceTotem(_activeShamanId);
            foreach (var totemUI in _totemUIHandlers.Where(totemUI => totemUI.ShamanId == _activeShamanId))
            {
                totemUI.ShowTotemPlaced();
            }
        }
    }
}