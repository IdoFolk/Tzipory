namespace Tzipory.ConfigFiles.WaveSystemConfig
{
    public interface IUpdateData<in T>
    {
        public void UpdateData(T data);
    }
}