

using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.Tools.Interface;

namespace Tzipory.ConfigFiles.WaveSystemConfig
{
    public interface ISerializeData : IInitialization<IConfigFile>
    {
        public int SerializeTypeId { get; }
    }
}