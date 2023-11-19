using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPanelUIManager : BaseUIElement
    {
        public static Dictionary<int,bool> TotemSelected { get; private set; }
        [SerializeField] private RectTransform _totemContainer;
        [SerializeField] private TotemUIHandler _totemUIHandler;
        [SerializeField] private TotemPlacementUI _totemPlacementUI;

        public TotemPlacementUI TotemPlacementUI => _totemPlacementUI;

        [SerializeField] private List<TotemUIHandler> _totemUIHandlers; //temp
        private bool _placementActive;

        public event Action<int> TotemClicked;
        public override void Init()
        {
            TotemSelected = new Dictionary<int, bool>();
            TotemManager.TotemPlaced += HideTotemUI;
            _totemPlacementUI.Init();
            foreach (var shaman in LevelManager.PartyManager.Party)
            {
                if (shaman.TotemConfig is null) return;
                var totemUI = Instantiate(_totemUIHandler, _totemContainer);
                totemUI.Init(shaman.TotemConfig,shaman.EntityInstanceID);
                _totemUIHandlers.Add(totemUI);
                TotemSelected.Add(shaman.EntityInstanceID,false);
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

        private void OnTotemClick(int shamanId) 
        {
            TotemClicked?.Invoke(shamanId);
        }

        public void HideTotemUI(int shamanId)
        {
            foreach (var totemUI in _totemUIHandlers.Where(totemUI => totemUI.ShamanId == shamanId))
            {
                totemUI.ShowTotemPlaced();
            }
        }

        public static void ToggleTotemSelected(int id, bool state)
        {
            TotemSelected[id] = state;
        }
        public static void ToggleAllTotemsSelected(bool state)
        {
            var keys = TotemSelected.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                TotemSelected[keys[i]] = state;
            }
        }
        public static void RemoveTotemSelected(int id) //on totem destroy
        {
            TotemSelected.Remove(id);
        }
    }
}