using System.Collections;
using System.Collections.Generic;
using GameplayeLogic.Nodes;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace Tzipory.Progression
{
    [System.Serializable]
    public class WorldMapProgression
    {
        [SerializeField] List<WorldMapNode> unlockedNodes;
        
        public void AddNodeStatus(WorldMapNode newBaseNode)
        {
            WorldMapNode currentWorldMapNode = unlockedNodes.Find(node => node.NodeID == newBaseNode.NodeID);
            if (currentWorldMapNode != null)
            {
                currentWorldMapNode.FillInfo(newBaseNode);
            }
            else
            {
                unlockedNodes.Add(newBaseNode);
            }
        }

        public WorldMapNode GetWorldMapNodeStatus(string nodeID)
        {
            WorldMapNode currentWorldMapNode = unlockedNodes.Find(node => node.NodeID == nodeID);
            return currentWorldMapNode;
        }
        
        //add check is node open
    }
}

