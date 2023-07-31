using System.Collections.Generic;
using SerializeData.Nodes;
using Systems.NodeSystem;

namespace SerializeData.Progression
{
    [System.Serializable]
    public class WorldMapProgressionSerializeData
    {
         List<WorldMapNodeSerializeData> unlockedNodes;

         public List<WorldMapNode> GetUnlockedWorldMapNodes()
         {
             List<WorldMapNode> worldMapNodes = new List<WorldMapNode>();
             
             foreach (WorldMapNodeSerializeData unlockedNode in unlockedNodes)
             {
                 if (unlockedNode is BattleMapNodeSerializeData battleMapNodeSerializeData)
                 {
                     worldMapNodes.Add(new BattleMapNode{ WorldMapNodeSerializeData = battleMapNodeSerializeData});;
                 }
                 else
                 {
                     worldMapNodes.Add(new WorldMapNode{ WorldMapNodeSerializeData = unlockedNode});;
                 }
             }
             return worldMapNodes;
         }
    }
}