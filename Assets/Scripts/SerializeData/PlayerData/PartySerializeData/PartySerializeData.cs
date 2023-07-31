using System.Collections.Generic;
using Helpers.Consts;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class PartySerializeData : ISerializeData
    {
        private List<ShamanDataContainer> _shamanDataContainers;
        
        public List<ShamanDataContainer> ShamanDataContainers => _shamanDataContainers;
        
        public bool IsInitialization { get; private set; }
        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;
        
        public void Init(IConfigFile parameter)
        {
            var config = (PartyConfig)parameter;
            
            _shamanDataContainers = new List<ShamanDataContainer>();

            foreach (var shamanConfig in config.PartyMembers)
            {
                var shamanSerializeData = DataManager.DataRequester.GetData<ShamanSerializeData>(shamanConfig);
                var shamanVisual =(ShamanConfig)DataManager.DataRequester.ConfigManager.GetConfig(shamanConfig.ConfigTypeId,
                    shamanConfig.ConfigObjectId);//temp!!!
                _shamanDataContainers.Add(new ShamanDataContainer(shamanSerializeData, shamanVisual.UnitEntityVisualConfig));
            }
            
            IsInitialization = true;
        }
    }
}