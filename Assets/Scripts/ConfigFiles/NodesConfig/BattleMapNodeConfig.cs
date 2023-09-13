using System.Collections.Generic;
using Tzipory.ConfigFiles.WaveSystemConfig;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.ConfigFiles.PartyConfig.NodesConfig
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
    [SerializeField] protected LevelConfig _levelToOpen;
    [SerializeField] private Sprite overrideSprite;

    public LevelConfig LevelToOpen => _levelToOpen;

    public Sprite OverrideSprite => overrideSprite;
}