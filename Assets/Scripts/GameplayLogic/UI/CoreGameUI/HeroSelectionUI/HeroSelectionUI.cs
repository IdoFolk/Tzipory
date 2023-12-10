using TMPro;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI
{
    public class HeroSelectionUI : MonoSingleton<HeroSelectionUI>, IInitialization<Shaman>
    {
        public bool IsInitialization { get; }

        [SerializeField] private Image _shamanSprite;
        [SerializeField] private TextMeshProUGUI _shamanName;
        [SerializeField] private StatBlockPanel _statBlockPanel;
        [SerializeField] private PSBonusUIHandler _psBonusUIHandler;

        public bool IsActive { get; private set; }

        public void Init(Shaman shaman)
        {
            _statBlockPanel.Init(shaman.Stats);
            _psBonusUIHandler.Show(shaman.Stats);
            _shamanSprite.sprite = shaman.VisualConfig.Icon;
            _shamanName.text = shaman.Name;
        }

        public void ShowSelectionUI(Shaman shaman)
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