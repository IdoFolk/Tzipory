using System;
using Helpers.Consts;
using SerializeData.Progression;
using Systems.DataManagerSystem;
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
        
//#if UNITY_EDITOR
        public void SetPartyData(ShamanConfig[] shamanConfigs)
        {
            PartySerializeData = new PartySerializeData();
            PartySerializeData.Init(shamanConfigs);
        }
//#endif
        
        public void Dispose()
        {
        }
    }
}