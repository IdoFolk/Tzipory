﻿using Tzipory.ConfigFiles;
using Tzipory.Helpers.Consts;

namespace Tzipory.SerializeData.PlayerData.Camp
{
    [System.Serializable]
    public class CampFacilitySerializeData : ISerializeData
    {
        private int _facilityID;
        private int _level;
        
        public int FacilityID => _facilityID;
        public int Level => _level;
        
        #region ISerializeData

        public int SerializeObjectId { get; }
        public int SerializeTypeId => Constant.DataId.CAMP_FACILITY_DATA_ID;
        
        public bool IsInitialization { get; }

        public CampFacilitySerializeData(int level, int facilityID)
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