using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.Party;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party
{
    [System.Serializable]
    public class PartySerializeData : ISerializeData , IInitialization<ShamanConfig[]>
    {
#if UNITY_EDITOR
        [SerializeField] private List<ShamanSerializeData> _shamanSerializeDatas;
#endif
        
        //all shamans
        private List<ShamanDataContainer> _shamanRosterDataContainers;
        
        private List<ShamanDataContainer> _shamansPartyDataContainers;
        
        public List<ShamanDataContainer> ShamansPartyDataContainers => _shamansPartyDataContainers;

        public List<ShamanDataContainer> ShamanRosterDataContainers => _shamanRosterDataContainers;
        
        public bool IsInitialization { get; private set; }
        
        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;
        
        public void Init(ShamanConfig[] parameter)//for testing
        {
#if UNITY_EDITOR
            _shamanSerializeDatas = new List<ShamanSerializeData>();
#endif
            _shamanRosterDataContainers = new List<ShamanDataContainer>();
            _shamansPartyDataContainers = new List<ShamanDataContainer>();
            
            foreach (var shamanConfig in parameter)
            {
                var shamanSerializeData = new ShamanSerializeData();
                shamanSerializeData.Init(shamanConfig);
                _shamanRosterDataContainers.Add(new ShamanDataContainer(shamanSerializeData, shamanConfig.UnitEntityVisualConfig));
            }

            _shamansPartyDataContainers = new List<ShamanDataContainer>();
            
            //Temp need this for the temp map system
            _shamansPartyDataContainers.AddRange(_shamanRosterDataContainers);
            
            IsInitialization = true;
        }
        
        public void Init(IConfigFile parameter)
        {
            var config = (PartyConfig)parameter;
            
#if UNITY_EDITOR
            _shamanSerializeDatas = new List<ShamanSerializeData>();
#endif
            
            _shamanRosterDataContainers = new List<ShamanDataContainer>();

            foreach (var shamanConfig in config.PartyMembers)
            {
                var shamanSerializeData = DataManager.DataRequester.GetSerializeData<ShamanSerializeData>(shamanConfig);
#if UNITY_EDITOR
                _shamanSerializeDatas.Add(shamanSerializeData);
#endif
                var shamanVisual =(ShamanConfig)DataManager.DataRequester.ConfigManager.GetConfig(shamanConfig.ConfigTypeId,
                    shamanConfig.ObjectId);//temp!!!
                _shamanRosterDataContainers.Add(new ShamanDataContainer(shamanSerializeData, shamanVisual.UnitEntityVisualConfig));
            }
            
            IsInitialization = true;
        }

        public void SetPartyMembers(List<ShamanDataContainer> shamanSerializeDataContainers)
        {
            _shamansPartyDataContainers = shamanSerializeDataContainers;
        }
        
        public void AddPartyMember(int targetShamanID)
        {
            //Find the serializeData in the list
            ShamanDataContainer shamanContainerDataFromRoster = _shamanRosterDataContainers.Find(shamanDataContainer =>
                shamanDataContainer.ShamanSerializeData.ShamanId == targetShamanID);

            if (shamanContainerDataFromRoster == null)
            {
                Debug.LogError("Why did we try to add shaman that is not in the roster?");
                return;
            }

            if (!_shamansPartyDataContainers.Contains(shamanContainerDataFromRoster))
                _shamansPartyDataContainers.Add(shamanContainerDataFromRoster);
            else
            {
                Debug.LogError("Why did we try to add shaman that is already in the party?");
            }
        }

        public void RemovePartyMember(int targetShamanID)
        {
            ShamanDataContainer shamanContainerDataFromRoster = _shamansPartyDataContainers.Find(
                shamanDataContainer =>
                    shamanDataContainer.ShamanSerializeData.ShamanId == targetShamanID);
            if (shamanContainerDataFromRoster == null)
            {
                Debug.LogError("Why did we try to remove shaman that is not in the party?");
                return;
            }

            _shamansPartyDataContainers.Remove(shamanContainerDataFromRoster);
        }
    }
}