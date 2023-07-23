using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace Tzipory.Progression
{
    [System.Serializable]
    public class WorldMapProgression
    {
        [SerializeField] List<BaseNode> nodesStatus;

        public void AddNodeStatus(BaseNode newBaseNode)
        {
            BaseNode currentBaseNode = nodesStatus.Find(node => node.nodeID == newBaseNode.nodeID);
            if (currentBaseNode != null)
            {
                currentBaseNode.FillInfo(newBaseNode);
            }
            else
            {
                nodesStatus.Add(newBaseNode);
            }
        }

        public BaseNode GetNodeStatus(string nodeID)
        {
            BaseNode currentBaseNode = nodesStatus.Find(node => node.nodeID == nodeID);
            return currentBaseNode;
        }
    }
}

