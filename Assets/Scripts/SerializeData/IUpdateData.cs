namespace Tzipory.SerializeData
{
    public interface IUpdateData<in T>
    {
        public void UpdateData(T data);
    }
}