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
        
        public Sprite OverrideSprite
        {
            get { return overrideSprite; }
        }

        [SerializeField] protected LevelConfig level;
        [SerializeField] protected BattleMapNodeState startingNodeState;
        [SerializeField] private Sprite overrideSprite;
    }
}