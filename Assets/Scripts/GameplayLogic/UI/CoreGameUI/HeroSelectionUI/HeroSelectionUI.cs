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
        [SerializeField] private StatBlockPanel _statBlockPanel;
        
        public bool IsActive { get; private set; }
        public void Init(Shaman shaman)
        {
            _statBlockPanel.Init(shaman.Stats);
            _shamanSprite.sprite = shaman.VisualConfig.Icon;
        }

        public void ShowSelectionUI(Shaman shaman)
        {
            Init(shaman);
            
            var Pos = transform.position;
            Pos.y += 150;
            transform.position = Pos;
            IsActive = true;
        }

        public void UpdateSelectionUI(Stat stat, float bonusValue)
        {
            _statBlockPanel.UpdateStatBlocks(stat, bonusValue);
        }

        public void HideSelectionUI()
        {
            var Pos = transform.position;
            Pos.y -= 150;
            transform.position = Pos;
            IsActive = false;
        }
    }
}