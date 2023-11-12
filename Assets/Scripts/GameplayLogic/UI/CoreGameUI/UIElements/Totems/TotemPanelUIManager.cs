using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPanelUIManager : BaseUIElement
    {
        [SerializeField] private RectTransform _totemContainer;
        [SerializeField] private TotemUIHandler _totemUIHandler;
        [SerializeField] private TotemPlacementUIHandler _totemPlacementUIHandler;

        [SerializeField] private List<Totem> _totemsSerialize; //temp
        [SerializeField] private List<TotemUIHandler> _totemUIHandlers; //temp
        private bool _placementActive;
        private int _activeTotemId;
        public override void Init()
        {
            if (_totemsSerialize is null) return;
            for (int i = 0; i < _totemsSerialize.Count; i++)
            {
                var totemUI = Instantiate(_totemUIHandler, _totemContainer);
                totemUI.SetTotemData(_totemsSerialize[i]);
                totemUI.Init(i);
                _totemUIHandlers.Add(totemUI);
                totemUI.OnTotemClick += OnTotemClick;
            }
            base.Init();
        }
        //add unsubscribe to event
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

        private void OnTotemClick(int id)
        {
            _totemPlacementUIHandler.ToggleSprite(true);
            _placementActive = true;
            _activeTotemId = id;
        }

        private void PlaceTotem()
        {
            if (_totemsSerialize[_activeTotemId] is null) return;
            TotemsManager.PlaceTotem(_totemsSerialize[_activeTotemId]);
            _totemUIHandlers[_activeTotemId].ActivateCooldown();
        }
    }
}