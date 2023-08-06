using System;
using Tzipory.SerializeData;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.CampSystem
{
    public class ShamanPartyMemberSelectUI : MonoBehaviour
    {
        public ShamanSerializeData AssociatedShamanData
        {
            get { return _associatedShamanData; }
        }
        public Toggle toggle;
        public Image shamanImage;

        public Action<ShamanSerializeData> onToggleChanged;
        
        private ShamanSerializeData _associatedShamanData;

        public void SetShamanData(ShamanSerializeData newShamanData)
        {
            _associatedShamanData = newShamanData;
        }
        
        public void ToggleChanged(bool isActive)
        {
            onToggleChanged?.Invoke(_associatedShamanData);
        }
    }
}