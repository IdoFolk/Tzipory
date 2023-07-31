using SerializeData.Nodes;

namespace Systems.NodeSystem
{
    public class WorldMapNode : BaseNode
    {
        //ask yonatan this too
        public WorldMapNodeSerializeData WorldMapNodeSerializeData
        {
            get { return worldMapNodeSerializeData; }
            set { worldMapNodeSerializeData = value; }
        }
        private WorldMapNodeSerializeData worldMapNodeSerializeData;
        
        public override void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            base.FillInfo(newBaseNodeSerializeData);
            var serializeData = GetConfig<WorldMapNodeSerializeData>(newBaseNodeSerializeData);
        }
        
    }
}