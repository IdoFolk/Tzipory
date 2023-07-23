using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace GameplayeLogic.Nodes.Config
{
    [CreateAssetMenu(fileName = "BaseNode", menuName = "ScriptableObjects/Nodes/BaseNode", order = 1)]
    public class BaseNodeSO : ScriptableObject
    {
        public BaseNode node;
    }
}
