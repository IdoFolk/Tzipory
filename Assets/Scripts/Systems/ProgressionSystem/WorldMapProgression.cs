using System.Collections;
using System.Collections.Generic;
using SerializeData.Nodes;
using SerializeData.Progression;
using Systems.NodeSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.Progression
{
    public class WorldMapProgression : IInitialization<WorldMapProgressionSerializeData>
    {
        public IReadOnlyList<WorldMapNode> UnlockedNodes => unlockedNodes;
        private List<WorldMapNode> unlockedNodes;

        public bool IsInitialization { get; }
        
        public void Init(WorldMapProgressionSerializeData parameter)
        {
            unlockedNodes = parameter.GetUnlockedWorldMapNodes();
        }
        
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

