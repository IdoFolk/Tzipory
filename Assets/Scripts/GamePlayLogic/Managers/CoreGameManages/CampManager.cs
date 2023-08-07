using System;
using System.Collections.Generic;
using Helpers.Consts;
using Systems.CampSystem;
using Systems.DataManagerSystem;
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
        //Should stay here?//MAKE CampUIManager
        #region UI
        [Header("UI")] 
        public ShamanPartyMemberSelectUI[] shamanToggles;
        #endregion

        #region Buildings

        [Header("Buildings")]
        public CampBuildingObject[] campBuildingObjects;
        #endregion
        
        public CampSerializeData CampSerializeData => _campSerializeData;
        private CampSerializeData _campSerializeData;

        public event Action onGraphicsRefresh;
        
        public bool IsInitialization { get; private set; }

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
            shamanToggles[0].SetShamanData(shamanSerializeData);
            shamanToggles[1].SetShamanData(new ShamanSerializeData(){_shamanId = 2});
            shamanToggles[2].SetShamanData(new ShamanSerializeData(){_shamanId = 3});
            // foreach (ShamanPartyMemberSelectUI shamanPartyMemberSelectUI in shamanToggles)
            // {
            //     
            // }
            
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

        public void ApplyPartyMembers()
        {
            //Make it happan witch each click
            List<ShamanSerializeData> selectedShamans = GetSelectedShamans();
            GameManager.PlayerManager.PlayerSerializeData.SetPartyMembers(selectedShamans);
        }

        public void AttachItemToShaman(ShamanSerializeData shamanToAttach, ShamanItemSerializeData itemToAttach)
        {
            shamanToAttach.AttachItem(itemToAttach);
        }
        
        [ContextMenu("Attach item to shaman")]
        public void AttachItemToShamanTest()
        {
            AttachItemToShaman(shamanToggles[1].AssociatedShamanData, new ShamanItemSerializeData());
        }
        
        List<ShamanSerializeData> GetSelectedShamans()
        {
            //Change to whatever the ui is setting which shamans to take
            List<ShamanSerializeData> selectedShamans = new List<ShamanSerializeData>();
            foreach (ShamanPartyMemberSelectUI shamanPartyMemberSelectUI in shamanToggles)
            {
                if (shamanPartyMemberSelectUI.toggle.isOn)
                {
                    selectedShamans.Add(shamanPartyMemberSelectUI.AssociatedShamanData);
                }
            }

            return selectedShamans;
        }
        #endregion

        
        public void Dispose()
        {
             
        }


    }
}