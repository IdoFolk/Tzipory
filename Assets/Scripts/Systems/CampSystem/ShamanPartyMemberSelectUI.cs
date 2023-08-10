using System;
using Tzipory.SerializeData;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.CampSystem
{
    public class ShamanPartyMemberSelectUI : MonoBehaviour
    {
        //change it to consts
        public int AssociatedShamanID
        {
            get { return _associatedShamanID; }
        }
        public Toggle toggle;
        public Image shamanImage;

        public Action<int, bool> onToggleChanged;
        
        private int _associatedShamanID;

        public void SetShamanID(int newShamanID)
        {
            _associatedShamanID = newShamanID;
        }
        
        public void ToggleChanged(bool isActive)
        {
            onToggleChanged?.Invoke(_associatedShamanID,isActive);
        }
    }
}