using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.NodesConfig;
using Tzipory.Helpers.Consts;

namespace Tzipory.SerializeData.NodeSerializeData
{
    [System.Serializable]
    public abstract class BaseNodeSerializeData : ISerializeData
    {
        private bool _visitedByPlayer;
        public int NodeId { get; protected set; }
        public bool IsInitialization { get; protected set; }
        public int SerializeObjectId { get; }
        public int SerializeTypeId => Constant.DataId.NODE_DATA_ID;

        public virtual void Init(IConfigFile parameter)
        {
            var config = (BaseNodeConfig)parameter;
            
            NodeId = config.ObjectId;
        }
    }
}
