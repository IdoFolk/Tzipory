namespace Tzipory.ConfigFiles
{
    public interface IConfigFile
    {
        public int ConfigObjectId { get; }
        public int ConfigTypeId { get; }
    }
}