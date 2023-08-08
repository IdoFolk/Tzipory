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
        //all shamans
        private List<ShamanDataContainer> _shamanDataContainers;
        
        public List<ShamanDataContainer> ShamanDataContainers => _shamanDataContainers;

        //Add selected current party here + change to container
        private List<ShamanSerializeData> _shamansInPartySerializeDatas = new List<ShamanSerializeData>();

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

            _shamansInPartySerializeDatas = new List<ShamanSerializeData>();
            
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

        public void SetPartyMembers(List<ShamanSerializeData> shamanSerializeDatas)
        {
            _shamansInPartySerializeDatas = shamanSerializeDatas;
        }

        //send here the id
        public void AddPartyMember(ShamanSerializeData targetShaman)
        {
            //Find the serializeData in the list
            ShamanSerializeData shamanSerializeDataInParty = _shamansInPartySerializeDatas.Find(shamanSerializeData => shamanSerializeData.ShamanId == targetShaman.ShamanId);
            if (shamanSerializeDataInParty == null)
            {
                _shamansInPartySerializeDatas.Add(targetShaman);
            }
            else
            {
                Debug.LogError("Why did we try to add shaman that is already in the party?");
            }
        }
        
        public void RemovePartyMember(ShamanSerializeData targetShaman)
        {
            ShamanSerializeData shamanSerializeDataInParty = _shamansInPartySerializeDatas.Find(shamanSerializeData => shamanSerializeData.ShamanId == targetShaman.ShamanId);
            if (shamanSerializeDataInParty != null)
            {
                _shamansInPartySerializeDatas.Remove(shamanSerializeDataInParty);
            }
            else
            {
                Debug.LogError("Why did we try to remove shaman that is not in the party?");
            }
        }
    }
}