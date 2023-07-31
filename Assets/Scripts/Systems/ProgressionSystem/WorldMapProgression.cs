using System.Collections;
using System.Collections.Generic;
using SerializeData.Nodes;
using Systems.NodeSystem;
using UnityEngine;

namespace Tzipory.Progression
{
    [System.Serializable]
    public class WorldMapProgression
    {
        public IReadOnlyList<WorldMapNode> UnlockedNodes => unlockedNodes;
        private List<WorldMapNode> unlockedNodes;
        
        public void AddNodeStatus(WorldMapNodeSerializeData newBaseNodeSerializeData)
        {
            WorldMapNode currentWorldMapNodeSerializeData = 
                unlockedNodes.Find(node => node.WorldMapNodeSerializeData.NodeID == newBaseNodeSerializeData.NodeID);
            if (currentWorldMapNodeSerializeData != null)
            {
                currentWorldMapNodeSerializeData.FillInfo(newBaseNodeSerializeData);
            }
            else
            {
                unlockedNodes.Add(new WorldMapNode()
                {
                    WorldMapNodeSerializeData =  newBaseNodeSerializeData
                });
            }
        }

        public WorldMapNode GetWorldMapNodeStatus(string nodeID)
        {
            WorldMapNode worldMapNode =  
                unlockedNodes.Find(node => node.WorldMapNodeSerializeData.NodeID == nodeID);
            return worldMapNode;
        }

        public bool IsNodeUnlocked(string nodeID)
        {
           return unlockedNodes.Exists(node => node.WorldMapNodeSerializeData.NodeID == nodeID);
        }
        
    }
}

