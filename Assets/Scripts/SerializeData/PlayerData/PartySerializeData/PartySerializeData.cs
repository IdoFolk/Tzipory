using System.Collections.Generic;
using Helpers.Consts;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class PartySerializeData : ISerializeData , IInitialization<ShamanConfig[]>
    {
        private List<ShamanDataContainer> _shamanDataContainers;
        
        public List<ShamanDataContainer> ShamanDataContainers => _shamanDataContainers;
        
        public bool IsInitialization { get; private set; }
        
        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;
        
        public void Init(ShamanConfig[] parameter)//for testing
        {
            _shamanDataContainers = new List<ShamanDataContainer>();

            foreach (var shamanConfig in parameter)
            {
                var shamanSerializeData = new ShamanSerializeData();
                shamanSerializeData.Init(shamanConfig);
                _shamanDataContainers.Add(new ShamanDataContainer(shamanSerializeData, shamanConfig.UnitEntityVisualConfig));
            }
            
            IsInitialization = true;
        }
        
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