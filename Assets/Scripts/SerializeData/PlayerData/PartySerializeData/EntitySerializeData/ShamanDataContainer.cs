
using Tzipory.ConfigFiles.EntitySystem.EntityVisual;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    public class ShamanDataContainer
    {
        public ShamanSerializeData ShamanSerializeData { get; }
        public BaseUnitEntityVisualConfig UnitEntityVisualConfig { get; }

        public ShamanDataContainer(ShamanSerializeData shamanSerializeData, BaseUnitEntityVisualConfig unitEntityVisualConfig)
        {
            ShamanSerializeData = shamanSerializeData;
            UnitEntityVisualConfig = unitEntityVisualConfig;
        }
    }
}