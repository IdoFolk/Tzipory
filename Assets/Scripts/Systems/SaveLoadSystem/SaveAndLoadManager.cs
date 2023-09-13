using System.Collections.Generic;
using Tzipory.ConfigFiles.WaveSystemConfig;

namespace Systems.SaveLoadSystem
{
    public class SaveAndLoadManager
    {
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