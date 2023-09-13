using Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.PartyConfig
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "ScriptableObjects/Config/Player config", order = 0)]
    public class PlayerConfig : ScriptableObject , IConfigFile
    {
        public int ConfigObjectId { get; }
        
        public int ConfigTypeId => Constant.DataId.PLAYER_DATA_ID;

        [SerializeField] private PartyConfig _partyConfig;
        
        public PartyConfig PartyConfig => _partyConfig;
    }
}