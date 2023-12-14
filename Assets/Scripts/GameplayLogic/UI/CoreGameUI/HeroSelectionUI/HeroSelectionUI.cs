using System.Collections.Generic;
using TMPro;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class HeroSelectionUI : MonoSingleton<HeroSelectionUI>, IInitialization<UnitEntity>
    {
        public bool IsInitialization { get; }

        [SerializeField] private Image _shamanSprite;
        [SerializeField] private TextMeshProUGUI _shamanName;
        [SerializeField] private StatBlockPanel _statBlockPanel;
        [SerializeField] private PSBonusUIHandler _psBonusUIHandler;

        public bool IsActive { get; private set; }

        public void Init(UnitEntity shaman)
        {
            IEnumerable<Stat> stats = shaman.EntityStatComponent.GetAllStats();
            
            _statBlockPanel.Init(stats);
            _psBonusUIHandler.Show(stats);
            _shamanSprite.sprite = shaman.Config.VisualComponentConfig.Icon;
            _shamanName.text = shaman.Config.EntityName;
        }

        public void ShowSelectionUI(UnitEntity shaman)
        {
            Init(shaman);

            var Pos = transform.position;
            Pos.y += 180;
            transform.position = Pos;
            IsActive = true;
        }

        public void UpdateStatBlocks(Stat shamanStat, float newValue)
        {
            _statBlockPanel.UpdateStatBlocks(shamanStat, newValue);
        }

        public void HideSelectionUI()
        {
            _statBlockPanel.HideStatBlocks();
            _psBonusUIHandler.Hide();
            var Pos = transform.position;
            Pos.y -= 180;
            transform.position = Pos;
            IsActive = false;
        }
    }
}