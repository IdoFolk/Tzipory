using System;
using Tzipory.GameplayLogic.StatusEffectTypes.Nodes;

namespace Systems.NodeSystem
{
    public abstract class BaseNode
    {
        public BaseNodeSerializeData NodeSerializeData { get; private set; }

        public int NodeId { get; private set; }
        
        public virtual void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            NodeSerializeData = newBaseNodeSerializeData;
            NodeId  = newBaseNodeSerializeData.NodeId;
        }
        
        protected T GetConfig<T>(BaseNodeSerializeData effectActionConfig) where T : BaseNodeSerializeData
        {
            if (effectActionConfig is T effectActionSo)
                return  effectActionSo;

            throw new Exception($"Can't cast {effectActionConfig.GetType()} to {typeof(T)}");
        }
        
    }
} 