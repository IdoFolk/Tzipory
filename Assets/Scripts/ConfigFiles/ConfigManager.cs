using Tzipory.Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig;
using UnityEngine;


namespace Tzipory.ConfigFiles.PartyConfig
{
    [System.Serializable]
    public class ConfigManager
    {
        [SerializeField] private ConfigHandler<ShamanConfig> _shamanConfig;
        [SerializeField] private ConfigHandler<EnemyConfig> _enemyConfig;
        [SerializeField] private ConfigHandler<AbilityConfig> _abilityConfig;
        [SerializeField] private ConfigHandler<PartyConfig> _partyConfig;
        //add item config handler
        
        
        public IConfigFile GetConfig(int dataId,int objectId)
        {
            switch (dataId)
            {
                case Constant.DataId.SHAMAN_DATA_ID:
                    return _shamanConfig.GetConfigFile(objectId);
                case Constant.DataId.ENEMY_DATA_ID:
                    return _enemyConfig.GetConfigFile(objectId);
                case Constant.DataId.ABILITY_DATA_ID:
                    return _abilityConfig.GetConfigFile(objectId);
                case Constant.DataId.PARTY_DATA_ID:
                    return _partyConfig.GetConfigFile(objectId);
                
                default:
                    return null;
            }
        }
    }
}