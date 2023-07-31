using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SerializeData.Nodes
{
    [System.Serializable]
    public abstract class BaseNodeSerializeData
    {
        public string NodeID
        {
            get { return nodeID; }
            set => nodeID = value;
        }
        
        public string nodeID;
        private bool visitedByPlayer;


    }
}
