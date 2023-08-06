using System;
using Helpers.Consts;
using SerializeData.Progression;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;

namespace GameplayeLogic.Managersp
{
    public class PlayerSerializeData : ISerializeData, IDisposable
    {
        //staemID
        
        //eficId
        //TODO when changing party members, change it here
        private PartySerializeData _partySerializeData;
        private CampSerializeData _campSerializeData;
        private WorldMapProgressionSerializeData _mapProgressionSerializeData;

        public PartySerializeData PartySerializeData => _partySerializeData;
        public CampSerializeData CampSerializeData => _campSerializeData;
        public WorldMapProgressionSerializeData WorldMapProgressionSerializeData => _mapProgressionSerializeData;

        public bool IsInitialization { get; private set; }
        public int SerializeTypeId => Constant.DataId.PLAYER_DATA_ID;
        
        public void Init(IConfigFile parameter)
        {
            var config = (PlayerConfig)parameter;
            _partySerializeData = DataManager.DataRequester.GetData<PartySerializeData>(config.PartyConfig);
            
            IsInitialization = true;
        }
        
        public void Dispose()
        {
        }
    }
}