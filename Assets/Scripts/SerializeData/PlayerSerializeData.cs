using System;
using Helpers.Consts;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;

namespace GameplayeLogic.Managersp
{
    public class PlayerSerializeData : ISerializeData, IDisposable
    {
        //staemID
        
        //eficId

        private PartySerializeData _partySerializeData;
        //camp serializeData 
        //map serializeData  

        public PartySerializeData PartySerializeData => _partySerializeData;

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