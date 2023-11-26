
using System;

namespace Tzipory.Systems.PoolSystem
{
    public interface IDisposable<out T> : IDisposable
    {
        public event Action<T> OnDispose;
    }
    public interface IPoolable<out T> : IDisposable<T>
    {
        public void Free();
    }
    
    public interface IPoolable<out T1 ,in T2> : IDisposable<T1>
    {
        public void Free();
    }
    
    public interface IPoolable<out T1, in T2, in T3> : IDisposable<T1>
    {
        public void Free();
    }
    
    public interface IPoolable<out T1,in T2, in T3, in T4> : IDisposable<T1>
    {
        public void Free();
    }
}