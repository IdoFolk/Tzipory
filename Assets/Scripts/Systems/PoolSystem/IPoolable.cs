﻿
using System;

namespace Tzipory.Systems.PoolSystem
{
    public interface IDisposable<out T>
    {
        public event Action<T> OnDispose;
        public void Dispose();
    }
    public interface IPoolable<out T> : IDisposable<T>
    {
        public void Reset();
        public void Free();
    }
    
    public interface IPoolable<out T1 ,in T2> : IDisposable<T1>
    {
        public void Reset(T2 t);
        public void Free();
    }
    
    public interface IPoolable<out T1, in T2, in T3> : IDisposable<T1>
    {
        public void Reset(T2 t1, T3 t2);
        public void Free();
    }
    
    public interface IPoolable<out T1,in T2, in T3, in T4> : IDisposable<T1>
    {
        public void Reset(T2 t1, T3 t2, T4 t3);
        public void Free();
    }
}