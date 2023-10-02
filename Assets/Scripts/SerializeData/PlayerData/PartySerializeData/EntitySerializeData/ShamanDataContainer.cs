using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig.EntityVisualConfig;

namespace Tzipory.SerializeData.PlayerData.PartySerializeData.EntitySerializeData
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