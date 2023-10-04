using System.Collections.Generic;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.NodesConfig
{
    [CreateAssetMenu(fileName = "BaseNode", menuName = "ScriptableObjects/Nodes/BaseNode", order = 1)]
    public abstract class BaseNodeConfig : ScriptableObject , IConfigFile
    {
        public List<BaseNodeConfig> UnlockNodes => new(_unlockNodes);
        public List<BaseNodeConfig> LeadToNodes => new(LeadToNodes);

        [Header("Base Node")] 
        [SerializeField] protected bool _isUnlock;
        [SerializeField] protected string _nodeName;
        [SerializeField] protected List<BaseNodeConfig> _leadToNodes;
        [SerializeField] protected List<BaseNodeConfig> _unlockNodes;
        [SerializeField] protected bool _leadAndUnlockNodesSame = true;
        public string NodeName => _nodeName;
        public bool IsUnlock => _isUnlock;
        public int ObjectId => name.GetHashCode();
        public int ConfigTypeId => Constant.DataId.NODE_DATA_ID;
    }
}
