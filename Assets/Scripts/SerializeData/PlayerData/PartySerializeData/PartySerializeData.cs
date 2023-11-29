using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.Player.Party;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party
{
    [System.Serializable]
    public class PartySerializeData : ISerializeData , IInitialization<UnitEntityConfig[]>
    {
        [SerializeField] private List<ShamanSerializeData> _shamanSerializeDatas;
        
        //all shamans
        
        public bool IsInitialization { get; private set; }

        public int SerializeObjectId { get; }
        public int SerializeTypeId => Constant.DataId.PARTY_DATA_ID;

        public List<ShamanSerializeData> ShamanSerializeDatas => _shamanSerializeDatas;

        public void Init(UnitEntityConfig[] parameter)//for testing
        {
            _shamanSerializeDatas = new List<ShamanSerializeData>();
            
            foreach (var shamanConfig in parameter)
            {
                var shamanSerializeData = new ShamanSerializeData();
                shamanSerializeData.Init(shamanConfig);
                _shamanSerializeDatas.Add(shamanSerializeData);
            }
            
            IsInitialization = true;
        }
        
        public void Init(IConfigFile parameter)
        {
            var config = (PartyConfig)parameter;
            
#if UNITY_EDITOR
            _shamanSerializeDatas = new List<ShamanSerializeData>();
#endif
            
            foreach (var shamanConfig in config.PartyMembers)
            {
                var shamanSerializeData = DataManager.DataRequester.GetSerializeData<ShamanSerializeData>(shamanConfig);
#if UNITY_EDITOR
                _shamanSerializeDatas.Add(shamanSerializeData);
#endif
            }
            
            IsInitialization = true;
        }
    }
}