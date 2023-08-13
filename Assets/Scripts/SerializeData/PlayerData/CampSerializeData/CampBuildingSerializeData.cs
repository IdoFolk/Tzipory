using System.Collections.Generic;
using Helpers.Consts;
using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.SerializeData
{
    public enum CampBuildingType
    {
        Workshop,
        Fireplace,
    }
    
    [System.Serializable]
    public class CampBuildingSerializeData : ISerializeData
    {
        private List<CampFacilitySerializeData> _campBuildingSubFacilitySerializeDatas;
        [SerializeField] private CampBuildingType _buildingType;
        
        public List<CampFacilitySerializeData> CampBuildingSubFacilitySerializeDatas =>
            _campBuildingSubFacilitySerializeDatas;
        
        public bool IsInitialization { get; private set; }

        public int SerializeTypeId => Constant.DataId.CAMP_BUILDING_DATA_ID;

        //use as id for factrory
        public CampBuildingType BuildingType => _buildingType;

        public int HighestFacilityLevel
        {
            get
            {
                int highestLevel = 0;
                foreach (CampFacilitySerializeData campFacilitySerializeData in _campBuildingSubFacilitySerializeDatas)
                {
                    if (campFacilitySerializeData.Level > highestLevel)
                        highestLevel = campFacilitySerializeData.Level;
                }

                return highestLevel;
            }
        }

        public void Init(IConfigFile parameter)
        {
           IsInitialization = true;
        }
        
        public CampBuildingSerializeData()
        {
            _campBuildingSubFacilitySerializeDatas = new List<CampFacilitySerializeData>();
        }
    }
}


