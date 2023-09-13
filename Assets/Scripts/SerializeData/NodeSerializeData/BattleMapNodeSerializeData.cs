using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.PartyConfig.NodesConfig;
using Tzipory.Tools.Enums;

namespace Tzipory.GameplayLogic.StatusEffectTypes.Nodes
{
    [System.Serializable]
    public class BattleMapNodeSerializeData : WorldMapNodeSerializeData
    {
        public BattleMapNodeState NodeState { get; private set; }
        public int LastBattleMoveIndex{ get; private set; }//may need to understand how to use it, last indirection whit the node 
        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            WorldMapNodeType  = WorldMapNodeType.BattleNode;
            var config = (BattleMapNodeConfig)parameter;
            NodeState = config.StartingNodeState;
            LastBattleMoveIndex = 0;
            IsInitialization = true;
        }
    }
}
