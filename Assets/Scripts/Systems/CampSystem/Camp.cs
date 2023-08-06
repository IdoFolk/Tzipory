using Tzipory.SerializeData;

namespace Systems.CampSystem
{
    public class Camp
    {
        public CampSerializeData CampSerializeData => _campSerializeData;
        private CampSerializeData _campSerializeData;

        public Camp(CampSerializeData campSerializeData)
        {
            _campSerializeData = campSerializeData;
        }

        public void UpgradeBuilding(CampBuildingType campBuildingType, int facilityID)
        {
            bool buildingFound = false;
            foreach (CampBuildingSerializeData campBuildingSerializeData in _campSerializeData.CampBuildingSerializeDatas)
            {
                if (campBuildingSerializeData.buildingType == campBuildingType)
                {
                    foreach (CampBuildingSubFacilitySerializeData campBuildingSubFacilitySerializeData
                             in campBuildingSerializeData.CampBuildingSubFacilitySerializeDatas)
                    {
                        if (campBuildingSubFacilitySerializeData.FacilityID == facilityID)
                        {
                            //TODO add material deduction
                            campBuildingSubFacilitySerializeData.Level++;
                            buildingFound = true;
                            break;
                        }
                    }
                }
                
                if(buildingFound)
                    break;
            }
        }
    }
}