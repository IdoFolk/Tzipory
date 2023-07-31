using Tzipory.SerializeData;

namespace Systems.CampSystem
{
    public enum CampBuildingType
    {
        Workshop,
        Fireplace,
    }
    
    public class CampBuilding
    {
        public  CampBuildingSerializeData CampBuildingSerializeData => _campBuildingSerializeData;
        private CampBuildingSerializeData _campBuildingSerializeData;
    }
}