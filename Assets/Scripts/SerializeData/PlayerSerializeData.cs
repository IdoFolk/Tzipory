﻿using System;
using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.InventorySerializeData;
using SerializeData.Progression;
using Systems.DataManagerSystem;
using Tools.Enums;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.SerializeData;
using UnityEngine;

namespace GameplayeLogic.Managers
{
    [Serializable]
    public class PlayerSerializeData : ISerializeData, IDisposable
    {
        //staemID
        
        //eficId

        [SerializeField] private int _currentWord;
        [SerializeField] private InventorySerializeData _inventorySerializeData;
        [SerializeField] private WorldMapProgressionSerializeData _worldMapProgression;
        [SerializeField] private PartySerializeData _partySerializeData;
        [SerializeField] private CampSerializeData _campSerializeData;
        
        public WorldMapProgressionSerializeData WorldMapProgression => _worldMapProgression;
        public PartySerializeData PartySerializeData => _partySerializeData;
        public CampSerializeData CampSerializeData => _campSerializeData;
        public InventorySerializeData InventorySerializeData => _inventorySerializeData;
        
        public bool IsInitialization { get; private set; }
        public int SerializeTypeId => Constant.DataId.PLAYER_DATA_ID;
        
        public void Init(IConfigFile parameter)
        {
            var config = (PlayerConfig)parameter;
            
            _partySerializeData = DataManager.DataRequester.GetData<PartySerializeData>(config.PartyConfig);
            _worldMapProgression = DataManager.DataRequester.GetData<WorldMapProgressionSerializeData>(_currentWord);
            _campSerializeData = DataManager.DataRequester.GetData<CampSerializeData>(Constant.DataId.CAMP_DATA_ID);
            _inventorySerializeData = DataManager.DataRequester.GetData<InventorySerializeData>(config.InventoryConfig);
            
            IsInitialization = true;
        }
        
//#if UNITY_EDITOR
        public void SetPartyData(ShamanConfig[] shamanConfigs)
        {
            _partySerializeData = new PartySerializeData();
            _partySerializeData.Init(shamanConfigs);
        }
//#endif
        
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
            // ShamanItemSerializeData shamanItemData = _itemsSerializeData.Find(itemData =>
            //     itemData.ItemInstanceId == targetItemInstanceID);
            //
            // if (shamanItemData == null)
            // {
            //     Debug.LogError("No item data found!");
            //     return;
            // }
            //
            // PartySerializeData.ToggleItemOnShaman(targetShamanID, shamanItemData, actionType);
        }
        
        // [Obsolete("Old method for setting party members")]
        // public void SetPartyMembers(List<ShamanSerializeData> shamanSerializeDatas)
        // {
        //     PartySerializeData.SetPartyMembers(shamanSerializeDatas);
        // }
    }
}