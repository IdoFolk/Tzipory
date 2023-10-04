using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData.NodeSerializeData
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
