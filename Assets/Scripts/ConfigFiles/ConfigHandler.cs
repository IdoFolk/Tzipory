using UnityEngine;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigHandler<T> where T : IConfigFile
    {
        [SerializeField] private T[] _config;
    }
}