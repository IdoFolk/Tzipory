using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigManager
    {
        [SerializeField] private ConfigHandler<ShamanConfig> _shamanConfig;
        [SerializeField] private ConfigHandler<EnemyConfig> _enemyConfig;
        [SerializeField] private ConfigHandler<AbilityConfig> _abilityConfig;

        public T GetConfig<T>(int objectId) where  T : class, IConfigFile
        {
            if(typeof(T) == typeof(ShamanConfig))
                return _shamanConfig.GetConfigFile(objectId) as T;

            if (typeof(T) == typeof(EnemyConfig))
                return _enemyConfig.GetConfigFile(objectId) as T;
            
            if (typeof(T) == typeof(AbilityConfig))
                return _abilityConfig.GetConfigFile(objectId) as T;

            Debug.LogError("ConfigManager.GetConfig() - Unknown config type: " + typeof(T));
            return null;
        }
    }
}