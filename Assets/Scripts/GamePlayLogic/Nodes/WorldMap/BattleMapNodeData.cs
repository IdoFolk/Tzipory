using Systems.NodeSystem.Config;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace GameplayeLogic.Nodes.Config
{
    [System.Serializable]
    public class BattleMapNodeData : BaseNodeData
    {
        public BattleMapNodeState StartingNodeState
        {
            get { return startingNodeState; }
        }
        
        [SerializeField] protected BattleMapNodeState startingNodeState;
    }
}