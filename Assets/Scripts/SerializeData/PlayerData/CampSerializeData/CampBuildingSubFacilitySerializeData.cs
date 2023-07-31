using System;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData 
{
    [System.Serializable]
    public class CampBuildingSubFacilitySerializeData : ISerializeData
    {

        public int FacilityID => facilityID;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        
        private int facilityID;
        private int level;

        #region ISerializeData

        public int SerializeTypeId { get; }
        
        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            //TODO add here
        }

        #endregion
     

     
    }
}