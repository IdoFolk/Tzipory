using Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.PartyConfig.NodesConfig;
using Tzipory.ConfigFiles.WaveSystemConfig;

namespace Tzipory.GameplayLogic.StatusEffectTypes.Nodes
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
