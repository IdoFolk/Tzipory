using System.Collections.Generic;
using System.Linq;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GamePlayLogic.UI
{
    public class CharacterRosterUI : BaseUIElement , IInitialization<IEnumerable<ShamanDataContainer>>
    {
        public event  System.Action<ShamanDataContainer> OnCharacterRosterSlotClicked;
    
        [SerializeField] private CharacterRosterSlotUI[] _rosterSlotUis;
        public bool IsInitialization { get; private set; }

        
        public void Init(IEnumerable<ShamanDataContainer> parameter)
        {
            ShamanDataContainer[] shamanDataContainers = parameter.ToArray();
            
            for (int i = 0; i < shamanDataContainers.Length; i++)
            {
                _rosterSlotUis[i].Init(shamanDataContainers[i]);
                _rosterSlotUis[i].OnCharacterRosterSlotClicked += ShamanSelected;
            }

            IsInitialization = true;
        }
    
        public override void Show()
        {
            base.Show();

            foreach (var rosterSlotUI in _rosterSlotUis)
            {
                if (rosterSlotUI.IsInitialization)
                     rosterSlotUI.Show();
            } 
        }

        private void ShamanSelected(ShamanDataContainer shamanDataContainer)
        {
            OnCharacterRosterSlotClicked?.Invoke(shamanDataContainer);
        }
        
        private void OnDestroy()
        {
            for (int i = 0; i < _rosterSlotUis.Length; i++)
            {
                if (_rosterSlotUis[i].IsInitialization)
                {
                    _rosterSlotUis[i].OnCharacterRosterSlotClicked -= ShamanSelected;
                }
            }
        }
    }

}
