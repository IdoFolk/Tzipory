using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig;
using UnityEngine;
using Constant = Tzipory.Helpers.Consts.Constant;

namespace Tzipory.ConfigFiles.PartyConfig
{
    [CreateAssetMenu(fileName = "NewPartyConfig", menuName = "ScriptableObjects/Config/New party config", order = 0)]
    public class PartyConfig : ScriptableObject , IConfigFile
    {
        [SerializeField] private int _configObjectId;
        [SerializeField] private ShamanConfig[] _partyMembers;

        public ShamanConfig[] PartyMembers => _partyMembers;

        public int ConfigObjectId => _configObjectId;
        
        public int ConfigTypeId => Constant.DataId.PARTY_DATA_ID;
    }
}