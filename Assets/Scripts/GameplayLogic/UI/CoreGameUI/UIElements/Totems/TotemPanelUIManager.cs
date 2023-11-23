using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.Systems.MovementSystem.HerosMovementSystem;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TotemPanelUIManager : BaseUIElement
    {
        public static Dictionary<int,bool> TotemSelected { get; private set; }
        [SerializeField] private RectTransform _totemContainer;
        [SerializeField] private TotemUIHandler _totemUIHandlerPrefab;
        [SerializeField] private TotemPlacementUI _totemPlacementUI;
        [SerializeField] private KeyCode[] _keybinds;
        public TotemPlacementUI TotemPlacementUI => _totemPlacementUI;

        [SerializeField] private List<TotemUIHandler> _totemUIHandlers;
        private bool _placementActive;

        public event Action<int> TotemClicked;
        public override void Init()
        {
            TotemSelected = new Dictionary<int, bool>();
            TotemManager.TotemPlaced += HideTotemUI;
            _totemPlacementUI.Init();
            for (int i = 0; i < LevelManager.PartyManager.Party.Length; i++)
            {
                var shaman = LevelManager.PartyManager.Party[i];
                if (shaman.TotemConfig is null) return;
                var totemUI = Instantiate(_totemUIHandlerPrefab, _totemContainer);
                totemUI.Init(shaman.TotemConfig,shaman.EntityInstanceID,_keybinds[i],i+1);
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

        public void ShowTotemUISelected(int shamanId)
        {
            foreach (var totemUIHandler in _totemUIHandlers.Where(totemUI => totemUI.ShamanId == shamanId))
            {
                totemUIHandler.ShowTotemSelected();
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