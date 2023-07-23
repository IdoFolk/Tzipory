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
        [SerializeField] List<WorldMapNode> nodesStatus;
        
        public void AddNodeStatus(WorldMapNode newBaseNode)
        {
            WorldMapNode currentWorldMapNode = nodesStatus.Find(node => node.nodeID == newBaseNode.nodeID);
            if (currentWorldMapNode != null)
            {
                currentWorldMapNode.FillInfo(newBaseNode);
            }
            else
            {
                nodesStatus.Add(newBaseNode);
            }
        }

        public WorldMapNode GetWorldMapNodeStatus(string nodeID)
        {
            WorldMapNode currentWorldMapNode = nodesStatus.Find(node => node.nodeID == nodeID);
            return currentWorldMapNode;
        }
    }
}

