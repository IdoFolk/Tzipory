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
        [SerializeField] List<WorldMapNode> unlockedNodes;
        
        public void AddNodeStatus(WorldMapNodeSerializeData newBaseNodeSerializeData)
        {
            WorldMapNodeSerializeData currentWorldMapNodeSerializeData = unlockedNodes.Find(node => node.NodeID == newBaseNodeSerializeData.NodeID);
            if (currentWorldMapNodeSerializeData != null)
            {
                currentWorldMapNodeSerializeData.FillInfo(newBaseNodeSerializeData);
            }
            else
            {
                unlockedNodes.Add(newBaseNodeSerializeData);
            }
        }

        public WorldMapNodeSerializeData GetWorldMapNodeStatus(string nodeID)
        {
            WorldMapNodeSerializeData currentWorldMapNodeSerializeData = unlockedNodes.Find(node => node.NodeID == nodeID);
            return currentWorldMapNodeSerializeData;
        }
        
        //add check is node open
    }
}

