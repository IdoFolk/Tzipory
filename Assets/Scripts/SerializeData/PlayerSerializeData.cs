using System;
using System.Collections.Generic;
using Helpers.Consts;
using NUnit.Framework;
using SerializeData.Progression;
using Systems.DataManagerSystem;
using Tools.Enums;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.SerializeData;
using UnityEngine;

namespace GameplayeLogic.Managersp
{
    [Serializable]
    public class PlayerSerializeData : ISerializeData, IDisposable
    {
        //staemID
        
        //eficId

        [SerializeField] private int _currentWord;
        public WorldMapProgressionSerializeData WorldMapProgression { get; private set; }
        public PartySerializeData PartySerializeData { get; private set; }
        
        private List<ShamanItemSerializeData> _itemsSerializeData = new List<ShamanItemSerializeData>();
        //camp serializeData 

        public bool IsInitialization { get; private set; }
        public int SerializeTypeId => Constant.DataId.PLAYER_DATA_ID;
        
        public void Init(IConfigFile parameter)
        {
            var config = (PlayerConfig)parameter;
            PartySerializeData = DataManager.DataRequester.GetData<PartySerializeData>(config.PartyConfig);
            WorldMapProgression = DataManager.DataRequester.GetData<WorldMapProgressionSerializeData>(_currentWord);
            
            IsInitialization = true;
        }
        
#if UNITY_EDITOR
        public void SetPartyData(ShamanConfig[] shamanConfigs)
        {
            PartySerializeData = new PartySerializeData();
            PartySerializeData.Init(shamanConfigs);
        }
#endif
        
        public void Dispose()
        {
            
        }
        
        //send him id
        public void ModifyPartyMember(int targetShamanID, CollectionActionType actionType)
        {
            if (actionType == CollectionActionType.Add)
            {
                PartySerializeData.AddPartyMember(targetShamanID);
            }
            else
            {
                PartySerializeData.RemovePartyMember(targetShamanID);
            }
        }
        
        public void ToggleItemOnShaman(int targetShamanID, int targetItemInstanceID,
            CollectionActionType actionType)
        {
            ShamanItemSerializeData shamanItemData = _itemsSerializeData.Find(itemData =>
                itemData.ItemInstanceId == targetItemInstanceID);

            if (shamanItemData == null)
            {
                Debug.LogError("No item data found!");
                return;
            }
            
            PartySerializeData.ToggleItemOnShaman(targetShamanID, shamanItemData, actionType);
        }
        
        // [Obsolete("Old method for setting party members")]
        // public void SetPartyMembers(List<ShamanSerializeData> shamanSerializeDatas)
        // {
        //     PartySerializeData.SetPartyMembers(shamanSerializeDatas);
        // }

      
    }
}