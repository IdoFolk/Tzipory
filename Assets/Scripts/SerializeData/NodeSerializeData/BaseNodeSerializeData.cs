using Tzipory.Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.PartyConfig.NodesConfig;
using Tzipory.SerializeData.PlayerData.PartySerializeData.EntitySerializeData;

namespace Tzipory.SerializeData.NodeSerializeData
{
    [System.Serializable]
    public abstract class BaseNodeSerializeData : ISerializeData
    {
        private bool _visitedByPlayer;
        public int NodeId { get; protected set; }
        public bool IsInitialization { get; protected set; }
        public int SerializeTypeId => Constant.DataId.NODE_DATA_ID;

        public virtual void Init(IConfigFile parameter)
        {
            var config = (BaseNodeConfig)parameter;
            
            NodeId = config.ConfigObjectId;
        }
    }
}
