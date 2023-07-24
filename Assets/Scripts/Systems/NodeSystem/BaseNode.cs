using SerializeData.Nodes;

namespace Systems.NodeSystem
{
    public abstract class BaseNode
    {
        private BaseNodeSerializeData baseNodeSerializeData;
        
        public virtual void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            baseNodeSerializeData.NodeID = newBaseNodeSerializeData.NodeID;
        }
    }
}