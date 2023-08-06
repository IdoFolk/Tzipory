using System;
using Helpers.Consts;
using Systems.CampSystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayeLogic.Managers
{
   
    public class CampManager : MonoBehaviour, IInitialization<CampSerializeData>
    {
        
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
                .Add(new CampBuildingSubFacilitySerializeData(0, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY));
            Init(campSerializeData);
        }

        public void Init(CampSerializeData parameter)
        {
            _campSerializeData = parameter;
            IsInitialization = true;
            onGraphicsRefresh?.Invoke();
        }

        public void UpgradeCampBuildingFacility(CampBuildingType campBuildingType, int facilityID)
        {
            _campSerializeData.UpgradeBuilding(campBuildingType, facilityID);
        }

        [ContextMenu("Upgrade Workshop Items Facility")]
        public void UpgradeWorkshopItemsTest()
        {
            //Simulating an upgrade from the UI for items facility in the workhshop building
            _campSerializeData.UpgradeBuilding(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY);
            Debug.Log(_campSerializeData.GetCampBuildingFacilityData(CampBuildingType.Workshop, Constant.CampBuildingFacilityId.WORKSHOP_ITEMS_FACILITY).Level);
        }
        
        public void Dispose()
        {
             
        }
    }
}