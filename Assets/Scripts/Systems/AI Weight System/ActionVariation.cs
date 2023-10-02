using UnityEngine;

namespace Tzipory.Systems.AIWeightSystem
{
    [System.Serializable]
    public class ActionVariation
    {
        public ActionComponent relevantItem; //list of actions/items
        public GameObject target; //list of targets?
    
        public int weight; 

        public ActionVariation(ActionComponent rItem, GameObject tgt, int actWeight)
        {
            relevantItem = rItem; //usually, the performing item will be the relevantItem
            target = tgt;
            weight = actWeight;
        }
        public void PerformActionOnTarget()
        {
            relevantItem.Action(this);
        }
    }
}

