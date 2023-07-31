using System.Collections.Generic;
using Systems.CampSystem;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData
{
    public class CampBuildingSerializeData : ISerializeData
    {
        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
           //TODO add here
        }

        public int SerializeTypeId { get; }

        public CampBuildingType buildingType;

        public List<CampBuildingSubFacilitySerializeData> CampBuildingSubFacilitySerializeDatas =>
            _campBuildingSubFacilitySerializeDatas;
        private List<CampBuildingSubFacilitySerializeData> _campBuildingSubFacilitySerializeDatas;
        
    }
}


