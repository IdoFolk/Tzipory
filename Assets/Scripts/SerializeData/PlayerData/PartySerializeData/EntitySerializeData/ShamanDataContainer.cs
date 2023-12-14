
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    public class ShamanDataContainer
    {
        public ShamanSerializeData ShamanSerializeData { get; }
        public VisualComponentConfig UnitEntityVisualConfig { get; }

        public ShamanDataContainer(ShamanSerializeData shamanSerializeData, VisualComponentConfig unitEntityVisualConfig)
        {
            ShamanSerializeData = shamanSerializeData;
            UnitEntityVisualConfig = unitEntityVisualConfig;
        }
    }
}