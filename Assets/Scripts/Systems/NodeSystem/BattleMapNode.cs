using Tzipory.SerializeData.NodeSerializeData;

namespace Tzipory.Systems.NodeSystem
{
    public class BattleMapNode : WorldMapNode
    {
        public BattleMapNodeSerializeData BattleMapNodeSerializeData { get; private set; }

        public override void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            base.FillInfo(newBaseNodeSerializeData);
             var serializeData = GetConfig<BattleMapNodeSerializeData>(newBaseNodeSerializeData);
             BattleMapNodeSerializeData = serializeData;
        }
    }
}