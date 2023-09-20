using Helpers.Consts;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Item;
using Tzipory.SerializeData;

namespace SerializeData.ItemSerializeData
{
    [System.Serializable]
    public class ItemContainerSerializeData : ISerializeData
    {
        public bool IsInitialization { get; private set; }

        public int ItemId;
        public int ItemStack;
        
        public void Init(IConfigFile parameter)
        {
            var config = (ItemConfig)parameter;

            ItemId = config.ObjectId;
            
            IsInitialization = true;
        }

        public int SerializeTypeId => Constant.DataId.ITEM_DATA_ID;
    }
}