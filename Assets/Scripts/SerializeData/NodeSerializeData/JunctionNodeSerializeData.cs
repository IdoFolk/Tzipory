
using Tzipory.ConfigFiles.PartyConfig;

namespace Tzipory.ConfigFiles.VisualSystemConfig.Nodes
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
