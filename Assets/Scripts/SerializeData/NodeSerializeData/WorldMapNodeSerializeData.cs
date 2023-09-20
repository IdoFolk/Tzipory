
namespace Tzipory.SerializeData.NodeSerializeData
{
    [System.Serializable]
    public abstract class WorldMapNodeSerializeData : BaseNodeSerializeData
    {
        public WorldMapNodeType WorldMapNodeType { get; protected set; }
    }

    public enum WorldMapNodeType
    {
        BattleNode,
        JunctionNode,
        QuestNode
    }
}