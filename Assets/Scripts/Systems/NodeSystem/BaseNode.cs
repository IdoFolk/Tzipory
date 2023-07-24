using System;
using SerializeData.Nodes;

namespace Systems.NodeSystem
{
    public abstract class BaseNode
    {
        private BaseNodeSerializeData baseNodeSerializeData;
        
        public virtual void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            baseNodeSerializeData.NodeID = newBaseNodeSerializeData.NodeID;
        }
        
        protected T GetConfig<T>(BaseNodeSerializeData effectActionConfig) where T : BaseNodeSerializeData
        {
            if (effectActionConfig is T effectActionSo)
                return  effectActionSo;

            throw new Exception($"Can't cast {effectActionConfig.GetType()} to {typeof(T)}");
        }
        
    }
} 