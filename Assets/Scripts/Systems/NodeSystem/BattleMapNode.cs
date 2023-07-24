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
             var serializeData = GetConfig<BattleMapNodeSerializeData>(newBaseNodeSerializeData);
             serializeData.nodeState = serializeData.nodeState;
             serializeData.lastBattleMoveIndex = serializeData.lastBattleMoveIndex;
            
        }
    }
}