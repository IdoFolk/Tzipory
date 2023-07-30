using System;
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
        
        public bool IsInitialization { get; private set; }
        public int SerializeTypeId { get; }
        
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