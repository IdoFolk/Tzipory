using SerializeData.Nodes;

namespace Systems.NodeSystem
{
    public class WorldMapNode : BaseNode
    {
        private WorldMapNodeSerializeData worldMapNodeSerializeData;
        
        public override void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            base.FillInfo(newBaseNodeSerializeData);
            if (newBaseNodeSerializeData is WorldMapNodeSerializeData worldMapNode)
            {
                //Add information only relvavnt to worldmap
            }
        }
        
    }
}