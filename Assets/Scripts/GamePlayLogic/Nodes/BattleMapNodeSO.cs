using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace GameplayeLogic.Nodes.Config
{
    [CreateAssetMenu(fileName = "BattleMapNode", menuName = "ScriptableObjects/Nodes/BattleMapNode", order = 2)]
    public class BattleMapNodeSO : BaseNodeSO
    {
        public BattleMapNodeData battleMapNode;
    }
}