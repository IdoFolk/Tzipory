using System.Collections.Generic;
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
        public List<CampBuildingSubFacilitySerializeData> CampBuildingSubFacilitySerializeDatas =>
            _campBuildingSubFacilitySerializeDatas;
        private List<CampBuildingSubFacilitySerializeData> _campBuildingSubFacilitySerializeDatas;
        
        public bool IsInitialization { get; }
        
        public int SerializeTypeId { get; }

        //use as id for factrory
        public CampBuildingType buildingType;

        public void Init(IConfigFile parameter)
        {
           
        }
        
        public CampBuildingSerializeData()
        {
            _campBuildingSubFacilitySerializeDatas = new List<CampBuildingSubFacilitySerializeData>();
        }

        
    }
}


