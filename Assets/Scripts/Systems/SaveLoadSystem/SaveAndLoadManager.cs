using System.Collections.Generic;
using Tzipory.SerializeData;

namespace Tzipory.Systems.SaveLoadSystem
{
    public class SaveAndLoadManager
    {
        public static string SAVE_SYSTEM_LOG_GROUP = "SaveSystem";
        
        public bool GetSaveData<T>(out T output) where T : class, ISerializeData, new()
        {
            //need to add save and load logic
            output = null;
            return false;
        }
        
        public bool GetSaveData<T>(out IEnumerable<T> output) where T : class, ISerializeData, new()
        {
            //need to add save and load logic
            output = null;
            return false;
        }
    }
}