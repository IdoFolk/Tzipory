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
        private List<ShamanDataContainer> _shamanDataContainers;
        
        public List<ShamanDataContainer> ShamanDataContainers => _shamanDataContainers;

        //Add selected current party here + change to container
        private List<ShamanDataContainer> _shamansInPartyDataContainers = new List<ShamanDataContainer>();
        
    

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

            _shamansInPartyDataContainers = new List<ShamanDataContainer>();
            
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

        public void SetPartyMembers(List<ShamanDataContainer> shamanSerializeDataContainers)
        {
            _shamansInPartyDataContainers = shamanSerializeDataContainers;
        }

        //send here the id
        public void AddPartyMember(int targetShamanID)
        {
            //Find the serializeData in the list
            ShamanDataContainer shamanContainerDataFromRoster = _shamanDataContainers.Find(shamanDataContainer =>
                shamanDataContainer.ShamanSerializeData.ShamanId == targetShamanID);

            if (shamanContainerDataFromRoster == null)
            {
                Debug.LogError("Why did we try to add shaman that is not in the roster?");
                return;
            }

            if (!_shamansInPartyDataContainers.Contains(shamanContainerDataFromRoster))
                _shamansInPartyDataContainers.Add(shamanContainerDataFromRoster);
            else
            {
                Debug.LogError("Why did we try to add shaman that is already in the party?");
            }
        }

        public void RemovePartyMember(int targetShamanID)
        {
            ShamanDataContainer shamanContainerDataFromRoster = _shamansInPartyDataContainers.Find(
                shamanDataContainer =>
                    shamanDataContainer.ShamanSerializeData.ShamanId == targetShamanID);
            if (shamanContainerDataFromRoster == null)
            {
                Debug.LogError("Why did we try to remove shaman that is not in the party?");
                return;
            }

            _shamansInPartyDataContainers.Remove(shamanContainerDataFromRoster);
        }

        public void ToggleItemOnShaman(int targetShamanID, ShamanItemSerializeData targetItemSerializeData,
            CollectionActionType actionType)
        {
            ShamanDataContainer shamanContainerDataFromRoster = _shamanDataContainers.Find(shamanDataContainer =>
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