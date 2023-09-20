using System;
using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.Nodes;
using Systems.NodeSystem;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;

namespace SerializeData.Progression
{
    [Serializable]
    public class WorldMapProgressionSerializeData : ISerializeData
    {
        private List<WorldMapNodeSerializeData> _unlockedNodes;
        
        public int CurrentNodeId { get; private set; }
        
        public int SerializeTypeId => Constant.DataId.MAP_DATA_ID;
        public bool IsInitialization { get; private set; }

        public void Init(IConfigFile parameter)
        {
            _unlockedNodes = new List<WorldMapNodeSerializeData>();
            IsInitialization  = true;
        }

        public void AddUnlockNode(WorldMapNodeSerializeData worldMapNodeSerializeData)
        {
            _unlockedNodes.Add(worldMapNodeSerializeData);
        }

        public List<WorldMapNode> GetUnlockedWorldMapNodes()
        {
            List<WorldMapNode> worldMapNodes = new List<WorldMapNode>();

            foreach (WorldMapNodeSerializeData unlockedNode in _unlockedNodes)
            {
                switch (unlockedNode.WorldMapNodeType)
                {
                    case WorldMapNodeType.BattleNode:
                        var battleNode = new BattleMapNode();
                        battleNode.FillInfo(unlockedNode);
                        worldMapNodes.Add(battleNode);
                        break;
                    case WorldMapNodeType.JunctionNode:
                        throw  new NotImplementedException();
                    case WorldMapNodeType.QuestNode:
                        throw  new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return worldMapNodes;
        }
    }
}