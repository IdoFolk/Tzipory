using Systems.NodeSystem.Config;
using Tzipory.Progression;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace GameplayeLogic.Nodes.Config
{
    [CreateAssetMenu(fileName = "BattleMapNode", menuName = "ScriptableObjects/Nodes/BattleMapNode", order = 2)]
    public class BattleMapNodeSO : BaseNodeSO
    {
        public BattleMapNodeData BattleMapNode
        {
            get { return battleMapNode; }
        }

        public Sprite OverrideSprite
        {
            get { return overrideSprite; }
        }
        
        [SerializeField] BattleMapNodeData battleMapNode;
        [SerializeField] private Sprite overrideSprite;
    }
}