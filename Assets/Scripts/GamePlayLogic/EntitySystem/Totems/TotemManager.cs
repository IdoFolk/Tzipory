using System;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.UIElements;
using Tzipory.Helpers;
using UnityEngine;


namespace Tzipory.GameplayLogic.EntitySystem.Totems
{
    public class TotemManager : MonoSingleton<TotemManager>
    {
        [SerializeField] private TotemPlacer _totemPlacer;
        [SerializeField] private TotemPanelUIManager _totemPanelUIManager;
        private TotemConfig _totemConfig;

        public TotemPanelUIManager TotemPanelUIManager => _totemPanelUIManager;

        public static event Action<int> TotemPlaced;

        private void Start()
        {
            _totemPlacer.Init();
            _totemPanelUIManager.TotemClicked += SelectTotem;
        }


        public void PlaceTotem(Vector3 pos, Shaman connectedShaman)
        {
            _totemPlacer.PlaceTotem(pos, connectedShaman.TotemConfig, connectedShaman);
            TotemPlaced?.Invoke(connectedShaman.EntityInstanceID);
        }

        public void SelectTotem(int shamanId)
        {
            foreach (var shaman in LevelManager.PartyManager.Party.Where(shaman => shaman.EntityInstanceID == shamanId))
            {
                TotemPanelUIManager.ToggleTotemSelected(shamanId,true);
                shaman.TempHeroMovement.SelectHero();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _totemPanelUIManager.TotemClicked -= SelectTotem;

        }
    }
}