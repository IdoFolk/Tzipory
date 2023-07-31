using System.Collections.Generic;
using GameplayeLogic.Managers;
using Systems.CampSystem;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class CampSerializeData : ISerializeData, IUpdateData<CampManager>
    {
        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
           //TODO add a lot
        }

        public int SerializeTypeId { get; }
        public void UpdateData(CampManager data)
        {
           //TODO add here logic
        }

        public List<CampBuildingSerializeData> CampBuildingSerializeDatas => _campBuildingSerializeDatas;
        
        private List<CampBuildingSerializeData> _campBuildingSerializeDatas;
    }
}