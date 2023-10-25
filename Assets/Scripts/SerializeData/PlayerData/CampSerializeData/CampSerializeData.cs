using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.GameplayLogic.Managers.MapManagers;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Camp
{
    [Serializable]
    public class CampSerializeData : ISerializeData, IUpdateData<CampManager>
    {
        private List<CampBuildingSerializeData> _campBuildingSerializeDatas;

        public List<CampBuildingSerializeData> CampBuildingSerializeDatas => _campBuildingSerializeDatas;
        
        public int SerializeTypeId { get; }
        public bool IsInitialization { get; private set; }
        
        public void Init(IConfigFile parameter)
        {
            
            IsInitialization  = true;
        }

        public void UpdateData(CampManager data)
        {
           
        }

        public CampSerializeData()
        {
            _campBuildingSerializeDatas = new List<CampBuildingSerializeData>();
        }

        public CampBuildingSerializeData GetCampBuildingData(CampBuildingType campBuildingType)
        {
            foreach (CampBuildingSerializeData campBuildingSerializeData in _campBuildingSerializeDatas)
            {
                if (campBuildingSerializeData.BuildingType == campBuildingType)
                {
                    return campBuildingSerializeData;
                }
            }

            Debug.LogError("Trying to retrieve a camp building that does not exist!");
            return null;
        }

        public CampFacilitySerializeData GetCampBuildingFacilityData(CampBuildingType campBuildingType,
            int facilityID)
        {
            CampBuildingSerializeData campBuildingSerializeData = GetCampBuildingData(campBuildingType);
            if (campBuildingSerializeData != null)
            {
                foreach (CampFacilitySerializeData campBuildingSubFacilitySerializeData
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
            CampFacilitySerializeData campFacilitySerializeData
                = GetCampBuildingFacilityData(campBuildingType, facilityID);

            if (campFacilitySerializeData != null)
            {
                campFacilitySerializeData.AddLevels(1);
            }
        }

    }
}