using System.Collections.Generic;
using Tzipory.SerializeData;
using Tzipory.SerializeData.PlayerData.Party.Entity;

namespace Tzipory.Systems.SaveLoadSystem
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