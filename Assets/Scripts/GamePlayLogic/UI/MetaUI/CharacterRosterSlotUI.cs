using System;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.GamePlayLogic.UI
{
    public class CharacterRosterSlotUI : BaseInteractiveUIElement , IInitialization<ShamanDataContainer>
    {
        public event Action<ShamanDataContainer> OnCharacterRosterSlotClicked;

        [SerializeField] private Image _image;
    
        private ShamanDataContainer _shamanDataContainer;
        public bool IsInitialization { get; private set; }


        public override void Show()
        {
            base.Show();
            _image.sprite = _shamanDataContainer.UnitEntityVisualConfig.Icon;
        }

        public void Init(ShamanDataContainer parameter)
        {
            _shamanDataContainer = parameter;
            IsInitialization = true;
            Show();
        }

        protected override void OnClick(PointerEventData eventData)
        {
            base.OnClick(eventData);
            OnCharacterRosterSlotClicked?.Invoke(_shamanDataContainer);
        }
    }
}

