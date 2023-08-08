﻿using System;
using System.Collections.Generic;
using GameplayeLogic.UIElements;
using Helpers.Consts;
using Systems.CampSystem;
using Systems.DataManagerSystem;
using Tools.Enums;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameplayeLogic.Managers
{
    public class CampManager : MonoBehaviour, IInitialization<CampSerializeData>
    {
     
        #region UI
        [Header("UI")]
        [SerializeField] private CampUIManager _campUIManager;
        #endregion

        #region Buildings

        [Header("Buildings")]
        public CampBuildingObject[] campBuildingObjects;
        #endregion
        
        public CampSerializeData CampSerializeData => _campSerializeData;
        private CampSerializeData _campSerializeData;

        public event Action onGraphicsRefresh;
        
        public bool IsInitialization { get; private set; }

        private ShamanItemSerializeData testingItem;
        private ShamanSerializeData testingCurrentShaman;
        
        //Here for TESTING!!
        private void Start()
        {
            CampSerializeData campSerializeData = new CampSerializeData();
            campSerializeData.CampBuildingSerializeDatas
                .Add(new CampBuildingSerializeData{buildingType = CampBuildingType.Workshop});
            campSerializeData.GetCampBuildingData(CampBuildingType.Workshop).CampBuildingSubFacilitySerializeDatas
                .Add(new CampFacilitySerializeData(0, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY));
            Init(campSerializeData);

            //How do we ask for data?
            //Just for testing, toggles will be created at runtime
            ShamanSerializeData shamanSerializeData = DataManager.DataRequester.GetData<ShamanSerializeData>(Constant.ShamanId.TOOR_ID);
            _campUIManager.shamanToggles[0].SetShamanData(shamanSerializeData);
            shamanSerializeData = DataManager.DataRequester.GetData<ShamanSerializeData>(Constant.ShamanId.JAVAN_ID);
            _campUIManager.shamanToggles[1].SetShamanData(shamanSerializeData);
            shamanSerializeData = DataManager.DataRequester.GetData<ShamanSerializeData>(Constant.ShamanId.NADIA_ID);
            _campUIManager.shamanToggles[2].SetShamanData(shamanSerializeData);

            foreach (ShamanPartyMemberSelectUI shamanPartyMemberSelectUI in _campUIManager.shamanToggles)
            {
                shamanPartyMemberSelectUI.onToggleChanged += PartyMemberToggleChanged;
            }
            testingItem = new ShamanItemSerializeData();
            testingItem._itemId = 10;
        }

        public void Init(CampSerializeData parameter)
        {
            _campSerializeData = parameter;
            IsInitialization = true;
            onGraphicsRefresh?.Invoke();
        }

        #region Buildings
        public void RefreshCampGraphics()
        {
            foreach (CampBuildingObject campBuildingObject in campBuildingObjects)
            {
                CampBuildingSerializeData campBuildingSerializeData =
                    _campSerializeData.GetCampBuildingData(campBuildingObject.campBuildingType);
                campBuildingObject.RefreshGraphic(campBuildingSerializeData.HighestFacilityLevel);
            }
        }
        
        public void UpgradeCampBuildingFacility(CampBuildingType campBuildingType, int facilityID)
        {
            _campSerializeData.UpgradeBuilding(campBuildingType, facilityID);
            //TODO add the invoke here
            onGraphicsRefresh.Invoke();
        }

        [ContextMenu("Upgrade Workshop Items Facility")]
        public void UpgradeWorkshopItemsTest()
        {
            //Simulating an upgrade from the UI for items facility in the workhshop building
            _campSerializeData.UpgradeBuilding(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY);
            Debug.Log(_campSerializeData.GetCampBuildingFacilityData(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY).Level);
        }

        #endregion

        #region Party
        
        public void TogglePartyMember(ShamanSerializeData targetShaman, CollectionActionType actionType)
        {
            GameManager.PlayerManager.PlayerSerializeData.TogglePartyMember(targetShaman, actionType);
        }

        [Obsolete("Old method for setting party members")]
        /// <summary>
        /// Old party members apply method
        /// </summary>
        public void ApplyPartyMembers()
        {
            //Make it happan witch each click
            List<ShamanSerializeData> selectedShamans = GetSelectedShamans();
            GameManager.PlayerManager.PlayerSerializeData.SetPartyMembers(selectedShamans);
        }

        void PartyMemberToggleChanged(ShamanSerializeData shamanSerializeData, bool isToggleActive)
        {
            TogglePartyMember(shamanSerializeData, isToggleActive ? CollectionActionType.Add : CollectionActionType.Remove);
        }
        
        List<ShamanSerializeData> GetSelectedShamans()
        {
            //Change to whatever the ui is setting which shamans to take
            List<ShamanSerializeData> selectedShamans = new List<ShamanSerializeData>();
            foreach (ShamanPartyMemberSelectUI shamanPartyMemberSelectUI in _campUIManager.shamanToggles)
            {
                if (shamanPartyMemberSelectUI.toggle.isOn)
                {
                    selectedShamans.Add(shamanPartyMemberSelectUI.AssociatedShamanData);
                }
            }

            return selectedShamans;
        }
        #endregion

        #region ShamanManagment

        public void ToggleItemOnShaman(ShamanSerializeData targetShaman, ShamanItemSerializeData targetItem,
            CollectionActionType actionType)
        {
            if (actionType == CollectionActionType.Add)
            {
                targetShaman.AttachItem(targetItem);
            }
            else
            {
                targetShaman.RemoveItem(targetItem);
            }
        }
        
        public void AttachItemToShamanTest()
        {
            ToggleItemOnShaman(_campUIManager.shamanToggles[0].AssociatedShamanData, testingItem, CollectionActionType.Add);
        }
        
        public void RemoveItemFromShamanTest()
        {
            ToggleItemOnShaman(_campUIManager.shamanToggles[0].AssociatedShamanData, testingItem, CollectionActionType.Remove);
        }

        #endregion
        
        public void Dispose()
        {
             
        }


    }
}