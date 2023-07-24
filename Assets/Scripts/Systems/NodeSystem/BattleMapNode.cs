using SerializeData.Nodes;

namespace Systems.NodeSystem
{
    public class BattleMapNode : WorldMapNode
    {
        public BattleMapNodeSerializeData BattleMapNodeSerializeData => battleMapNodeSerializeData;
        
        private BattleMapNodeSerializeData battleMapNodeSerializeData;
        
        public override void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            base.FillInfo(newBaseNodeSerializeData);
            if (newBaseNodeSerializeData is BattleMapNodeSerializeData battleMapNodeMapNode)
            {
                battleMapNodeSerializeData.nodeState = battleMapNodeMapNode.nodeState;
                battleMapNodeSerializeData.lastBattleMoveIndex = battleMapNodeMapNode.lastBattleMoveIndex;
            }
        }
    }
}