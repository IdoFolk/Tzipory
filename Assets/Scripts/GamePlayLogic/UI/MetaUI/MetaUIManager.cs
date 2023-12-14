using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.GamePlayLogic.UI.MetaUI
{
    public class MetaUIManager : MonoBehaviour
    {
        [SerializeField] private CampUIManager _campUIManager;
        [SerializeField] private CharacterRosterUI _characterRosterUI;

        private void Awake()
        {
            //_characterRosterUI.Init(GameManager.PlayerManager.PlayerSerializeData.PartySerializeData.ShamanRosterDataContainers);
            _characterRosterUI.OnCharacterRosterSlotClicked += _campUIManager.NewMainShamanSelected;
            _characterRosterUI.Show();
        }

        private void OnDestroy()
        {
            _characterRosterUI.OnCharacterRosterSlotClicked -= _campUIManager.NewMainShamanSelected;
        }
    }
}

