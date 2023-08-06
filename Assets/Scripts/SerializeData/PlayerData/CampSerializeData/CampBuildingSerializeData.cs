using System.Collections.Generic;
using Helpers.Consts;
using Systems.CampSystem;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData
{
    public enum CampBuildingType
    {
        Workshop,
        Fireplace,
    }
    public class CampBuildingSerializeData : ISerializeData
    {
        public List<CampFacilitySerializeData> CampBuildingSubFacilitySerializeDatas =>
            _campBuildingSubFacilitySerializeDatas;
        private List<CampFacilitySerializeData> _campBuildingSubFacilitySerializeDatas;
        
        public bool IsInitialization { get; }

        public int SerializeTypeId => Constant.DataId.CAMP_BUILD_DATA_ID;

        //use as id for factrory
        public CampBuildingType buildingType;

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
           
        }
        
        public CampBuildingSerializeData()
        {
            _campBuildingSubFacilitySerializeDatas = new List<CampFacilitySerializeData>();
        }

        
    }
}


