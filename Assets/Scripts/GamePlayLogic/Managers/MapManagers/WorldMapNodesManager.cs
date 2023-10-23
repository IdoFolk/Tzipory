using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.SerializeData.NodeSerializeData;
using Tzipory.SerializeData.ProgressionSerializeData;
using Tzipory.Systems.NodeSystem;
using Tzipory.Systems.SceneSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MapManagers
{
    public class WorldMapNodesManager : BaseNodeManager , IInitialization<WorldMapProgressionSerializeData>
    {
        [SerializeField] private NodeObject[] _nodeObjects;
        
        private NodeObject _currentNodeObject;
        private List<WorldMapNode> _unlockedNodes;

        public WorldMapProgressionSerializeData WorldMapProgressionSerializeData { get; private set; }

        public IReadOnlyList<WorldMapNode> UnlockedNodes => _unlockedNodes;
        
        public bool IsInitialization { get; private set; }
        
        private void Awake()
        {
            foreach (var nodeObject in _nodeObjects)
                nodeObject.OnNodeClick += NodeClicked;
            
            Init(GameManager.PlayerManager.PlayerSerializeData.WorldMapProgression);
        }
        
        public void Init(WorldMapProgressionSerializeData parameter)
        {
            WorldMapProgressionSerializeData = parameter;
            
            _unlockedNodes = parameter.GetUnlockedWorldMapNodes();
            TestNode();
            foreach (var nodeObject in _nodeObjects)
            {
                if (nodeObject.IsUnlock)
                {
                    nodeObject.Init();
                    var nodeData = new BattleMapNodeSerializeData();
                    nodeData.Init(nodeObject.BaseNodeConfig);
                    parameter.AddUnlockNode(nodeData);
                    continue;
                }
                
                foreach (var unlockedNode in _unlockedNodes)
                {
                    if (nodeObject.NodeId != unlockedNode.NodeId) continue;
                    
                    var battleNode = (BattleMapNode)unlockedNode;
                    nodeObject.Init(battleNode.BattleMapNodeSerializeData.NodeState);
                    break;
                }
                
                nodeObject.gameObject.SetActive(false);
            }
            
            IsInitialization = true;
        }

        [ContextMenu("Test node")]
        public void TestNode()
        {
            var nodeData = new BattleMapNodeSerializeData();
            nodeData.Init(_nodeObjects[2].BaseNodeConfig);
            AddNodeStatus(nodeData);
        }

        private void NodeClicked(BattleMapNodeStateConfig battleMapNodeStateConfig)//need to pass node data and not battleMapNodeStateConfig
        {
            GameManager.GameData.CurrentLevelConfig  = battleMapNodeStateConfig.LevelToOpen;
            GameManager.SceneHandler.LoadScene(SceneType.Game);//temp!
        }

        private void OnDestroy()
        {
            foreach (var nodeObject in _nodeObjects)
                nodeObject.OnNodeClick -= NodeClicked;
        }
        
        [Button("Refresh nodes")]
        private void OnValidate()
        {
            _nodeObjects = FindObjectsByType<NodeObject>(FindObjectsSortMode.None);
        }
        
        public void AddNodeStatus(WorldMapNodeSerializeData newBaseNodeSerializeData)
        {
            WorldMapNode worldMapNode = _unlockedNodes.Find(node => node.WorldMapNodeSerializeData.NodeId == newBaseNodeSerializeData.NodeId);
            if (worldMapNode != null)
            {
                worldMapNode.FillInfo(newBaseNodeSerializeData);
            }
            else
            {
                var node = new WorldMapNode();
                node.FillInfo(newBaseNodeSerializeData);
                _unlockedNodes.Add(node);
            }
        }

        public WorldMapNode GetWorldMapNodeStatus(int nodeID)
        {
            WorldMapNode worldMapNode =  
                _unlockedNodes.Find(node => node.WorldMapNodeSerializeData.NodeId == nodeID);
            return worldMapNode;
        }

        public bool IsNodeUnlocked(int nodeID)
        {
            return _unlockedNodes.Exists(node => node.WorldMapNodeSerializeData.NodeId == nodeID);
        }

    }
}
