namespace Tzipory.Systems.PoolSystem
{
    public interface IPoolTicket<out T>
    {
        public T GetObject();
    }
}