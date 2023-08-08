
using Tzipory.ConfigFiles;

namespace SerializeData.Nodes
{
    public class JunctionNodeSerializeData : WorldMapNodeSerializeData
    {
        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            WorldMapNodeType = WorldMapNodeType.JunctionNode;
        }
    }
}
