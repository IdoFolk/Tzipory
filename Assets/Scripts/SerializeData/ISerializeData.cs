

using Tzipory.ConfigFiles;
using Tzipory.Tools.Interface;

namespace Tzipory.SerializeData
{
    public interface ISerializeData : IInitialization<IConfigFile>
    {
        public int SerializeTypeId { get; }
    }
}