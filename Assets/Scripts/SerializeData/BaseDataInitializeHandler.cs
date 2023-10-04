using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public abstract class BaseDataInitializeHandler<T1,T2> where T1 : ISerializeData where T2 : IConfigFile
    {
        [SerializeField] private T2[] _configs;

        public abstract int DataTypeID { get; }

        public abstract T1 GetDataByName(string dataName);

        public abstract T1 GetDataById(int id);
    }
    
}