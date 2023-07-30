using System.Collections.Generic;
using Helpers.Consts;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class PartySerializeData : ISerializeData
    {
        private List<ShamanSerializeData> _shamanSerializeDatas;
        
        public List<ShamanSerializeData> ShamanSerializeDatas => _shamanSerializeDatas;
        
        public bool IsInitialization { get; }
        
        public void Init(IConfigFile parameter)
        {
            var config = (PartyConfig) parameter;
            
            _shamanSerializeDatas = new List<ShamanSerializeData>();
            
            foreach (var shamanConfig in config.PartyMembers)
                _shamanSerializeDatas.Add(DataManager.DataRequester.GetData<ShamanSerializeData>(shamanConfig));
        }

        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;
    }
}