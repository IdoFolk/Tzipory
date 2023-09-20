using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;
using Constant = Helpers.Consts.Constant;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class PartyConfig : IConfigFile
    {
        [SerializeField] private ShamanConfig[] _partyMembers;

        public ShamanConfig[] PartyMembers => _partyMembers;

        public int ObjectId => 0;
        
        public int ConfigTypeId => Constant.DataId.PARTY_DATA_ID;
    }
}