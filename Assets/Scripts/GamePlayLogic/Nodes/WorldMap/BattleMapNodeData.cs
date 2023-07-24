using Systems.NodeSystem.Config;
using Tzipory.SerializeData.LevalSerializeData;
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

        [SerializeField] protected LevelSerializeData level;
        [SerializeField] protected BattleMapNodeState startingNodeState;
    }
}