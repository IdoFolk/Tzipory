using System;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.CampSystem
{
    public class ShamanPartyMemberSelectUI : MonoBehaviour
    {
        public int AssociatedShamanID
        {
            get { return _associatedShamanID; }
        }
        public Toggle toggle;
        public Image shamanImage;

        public Action<int> onToggleChanged;

        //TODO asked yonatan if he prefer shamanSerialzeData
        private int _associatedShamanID;

        public void ToggleChanged(bool isActive)
        {
            onToggleChanged?.Invoke(_associatedShamanID);
        }
    }
}