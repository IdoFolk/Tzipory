using Helpers.Consts;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.Inventory;
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
            var config = (ItemContainerConfig)parameter;

            ItemId = config.ItemConfig.ObjectId;
            ItemStack = config.Amount;
            
            IsInitialization = true;
        }

        public int SerializeTypeId => Constant.DataId.ITEM_DATA_ID;
    }
}