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
        [SerializeField] private PartySerializeData _partySerializeData;
        [SerializeField] private CampSerializeData _campSerializeData;
        
        public PartySerializeData PartySerializeData => _partySerializeData;
        public CampSerializeData CampSerializeData => _campSerializeData;
        public InventorySerializeData InventorySerializeData => _inventorySerializeData;
        public WorldMapProgressionSerializeData WorldMapProgressionSerializeData { get; private set; }
        
        public bool IsInitialization { get; private set; }
        public int SerializeObjectId { get; }
        public int SerializeTypeId => Constant.DataId.PLAYER_DATA_ID;
        
        public void Init(IConfigFile parameter)
        {
            var config = (PlayerConfig)parameter;
            
            _partySerializeData = DataManager.DataRequester.GetSerializeData<PartySerializeData>(config.PartyConfig);
            _campSerializeData = DataManager.DataRequester.GetSerializeData<CampSerializeData>(Constant.DataId.CAMP_DATA_ID);
            WorldMapProgressionSerializeData = DataManager.DataRequester.GetSerializeData<WorldMapProgressionSerializeData>(Constant.DataId.MAP_DATA_ID);
            _inventorySerializeData = DataManager.DataRequester.GetSerializeData<InventorySerializeData>(config.InventoryConfig);
            
            IsInitialization = true;
        }
        
//#if UNITY_EDITOR
        public void SetPartyData(UnitEntityConfig[] shamanConfigs)
        {
            _partySerializeData = new PartySerializeData();
            _partySerializeData.Init(shamanConfigs);
        }
//#endif
        
        public void Dispose()
        {
            
        }
        
        //send him id
    }
}