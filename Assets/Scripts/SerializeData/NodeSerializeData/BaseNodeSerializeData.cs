using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SerializeData.Nodes
{
    [System.Serializable]
    public abstract class BaseNodeSerializeData
    {
      //  public string NodeID => nodeID;
        
        public string NodeID;
        private bool visitedByPlayer;


    }
}
