using System.Collections.Generic;
using Tzipory.Progression;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Nodes.Config
{
    [CreateAssetMenu(fileName = "BattleMapNode", menuName = "ScriptableObjects/Nodes/BattleMapNode", order = 2)]
    public class BattleMapNodeConfig : BaseNodeConfig
    {
        public BattleMapNodeState StartingNodeState
        {
            get { return startingNodeState; }
        }
        
        [Header("Battle Map Node")] 
        [SerializeField] protected BattleMapNodeState startingNodeState;
        [SerializeField] protected List<BattleMapNodeStateConfig> battleMapNodeStateConfigs;
        
        public BattleMapNodeStateConfig GetBattleMapNodeStateConfigByState(BattleMapNodeState battleMapNodeState)
        {
            return battleMapNodeStateConfigs.Find(nodeConfig => nodeConfig.nodeState == battleMapNodeState);
        }
    }
}

[System.Serializable]
//Ask yonatan for right name
public class BattleMapNodeStateConfig
{
    public BattleMapNodeState nodeState;
    [SerializeField] protected LevelConfig level;
    [SerializeField] private Sprite overrideSprite;
}