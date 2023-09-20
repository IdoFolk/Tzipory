﻿using Tzipory.SerializeData.NodeSerializeData;

namespace Tzipory.Systems.NodeSystem
{
    public class WorldMapNode : BaseNode
    {
        //ask yonatan this too
        public WorldMapNodeSerializeData WorldMapNodeSerializeData { get; private set; }

        public override void FillInfo(BaseNodeSerializeData newBaseNodeSerializeData)
        {
            base.FillInfo(newBaseNodeSerializeData);
            var serializeData = GetConfig<WorldMapNodeSerializeData>(newBaseNodeSerializeData);
            WorldMapNodeSerializeData = serializeData;
        }
    }
}