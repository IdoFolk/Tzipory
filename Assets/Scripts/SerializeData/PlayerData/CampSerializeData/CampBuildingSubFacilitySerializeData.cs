using System;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData 
{
    [System.Serializable]
    public class CampBuildingSubFacilitySerializeData : ISerializeData
    {
        public int FacilityID => _facilityID;

        public int Level
        {
            get { return _level; }
        }
        
        private int _facilityID;
        private int _level;

        #region ISerializeData

        public int SerializeTypeId { get; }
        
        public bool IsInitialization { get; }

        public CampBuildingSubFacilitySerializeData(int level, int facilityID)
        {
            _level = level;
            _facilityID = facilityID;
        }

        public void AddLevels(int levelsAmount)
        {
            _level += levelsAmount;
        }
        
        public void Init(IConfigFile parameter)
        {
            //TODO add here
        }

        #endregion
     

     
    }
}