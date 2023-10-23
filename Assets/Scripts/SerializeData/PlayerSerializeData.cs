using System;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.Player;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.Inventory;
using Tzipory.SerializeData.PlayerData.Camp;
using Tzipory.SerializeData.PlayerData.Party;
using Tzipory.SerializeData.ProgressionSerializeData;
using Tzipory.Systems.DataManager;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.SerializeData
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
            
            _partySerializeData = DataManager.DataRequester.GetSerializeData<PartySerializeData>(config.PartyConfig);
            _worldMapProgression = DataManager.DataRequester.GetSerializeData<WorldMapProgressionSerializeData>(_currentWord);
            _campSerializeData = DataManager.DataRequester.GetSerializeData<CampSerializeData>(Constant.DataId.CAMP_DATA_ID);
            _inventorySerializeData = DataManager.DataRequester.GetSerializeData<InventorySerializeData>(config.InventoryConfig);
            
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
    }
}