using System.Collections.Generic;
using GameplayeLogic.Managers;
using Systems.CampSystem;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class CampSerializeData : ISerializeData, IUpdateData<CampManager>
    {
        public List<CampBuildingSerializeData> CampBuildingSerializeDatas => _campBuildingSerializeDatas;

        private List<CampBuildingSerializeData> _campBuildingSerializeDatas;
        
        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
           //TODO add a lot
        }

        public int SerializeTypeId { get; }
        public void UpdateData(CampManager data)
        {
           //TODO add here logic
        }

        public CampSerializeData()
        {
            _campBuildingSerializeDatas = new List<CampBuildingSerializeData>();
        }

        public CampBuildingSerializeData GetCampBuildingData(CampBuildingType campBuildingType)
        {
            foreach (CampBuildingSerializeData campBuildingSerializeData in _campBuildingSerializeDatas)
            {
                if (campBuildingSerializeData.buildingType == campBuildingType)
                {
                    return campBuildingSerializeData;
                }
            }

            Debug.LogError("Trying to retrieve a camp building that does not exist!");
            return null;
        }

        public CampBuildingSubFacilitySerializeData GetCampBuildingFacilityData(CampBuildingType campBuildingType,
            int facilityID)
        {
            CampBuildingSerializeData campBuildingSerializeData = GetCampBuildingData(campBuildingType);
            if (campBuildingSerializeData != null)
            {
                foreach (CampBuildingSubFacilitySerializeData campBuildingSubFacilitySerializeData
                         in campBuildingSerializeData.CampBuildingSubFacilitySerializeDatas)
                {
                    if (campBuildingSubFacilitySerializeData.FacilityID == facilityID)
                    {
                        return campBuildingSubFacilitySerializeData;
                    }
                }
            }

            Debug.LogError("Trying to retrieve a camp building facility that does not exist!");
            return null;
        }

        public void UpgradeBuilding(CampBuildingType campBuildingType, int facilityID)
        {
            CampBuildingSubFacilitySerializeData campBuildingSubFacilitySerializeData
                = GetCampBuildingFacilityData(campBuildingType, facilityID);

            if (campBuildingSubFacilitySerializeData != null)
            {
                campBuildingSubFacilitySerializeData.AddLevels(1);
            }
        }

    }
}