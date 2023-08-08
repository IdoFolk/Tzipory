using Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig;

namespace Tzipory.SerializeData
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