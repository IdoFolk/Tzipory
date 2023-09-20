using System.Collections.Generic;
using Helpers.Consts;
using Systems.DataManagerSystem;
using Tools.Enums;
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
        private List<ShamanDataContainer> _shamanRosterDataContainers;

        private List<ShamanDataContainer> _shamansPartyDataContainers;
        
        public List<ShamanDataContainer> ShamansPartyDataContainers => _shamansPartyDataContainers;

        public List<ShamanDataContainer> ShamanRosterDataContainers => _shamanRosterDataContainers;



        public bool IsInitialization { get; private set; }
        
        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;
        
        public void Init(ShamanConfig[] parameter)//for testing
        {
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
            
            _shamanRosterDataContainers = new List<ShamanDataContainer>();

            foreach (var shamanConfig in config.PartyMembers)
            {
                var shamanSerializeData = DataManager.DataRequester.GetData<ShamanSerializeData>(shamanConfig);
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

        public void ToggleItemOnShaman(int targetShamanID, ShamanItemSerializeData targetItemSerializeData,
            CollectionActionType actionType)
        {
            ShamanDataContainer shamanContainerDataFromRoster = _shamanRosterDataContainers.Find(shamanDataContainer =>
                shamanDataContainer.ShamanSerializeData.ShamanId == targetShamanID);
            if (shamanContainerDataFromRoster == null)
            {
                Debug.LogError("Trying to toggle item on shaman who does not exist?");
                return;
            }
            
            if (actionType == CollectionActionType.Add)
            {
                shamanContainerDataFromRoster.ShamanSerializeData.AttachItem(targetItemSerializeData);
            }
            else
            {
                shamanContainerDataFromRoster.ShamanSerializeData.RemoveItem(targetItemSerializeData);
            }
        }
    }
}