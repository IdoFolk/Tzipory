using System;
using System.Collections.Generic;
using Tzipory.Systems.FactorySystem;

namespace Tzipory.Systems.PoolSystem
{
    public class ObjectPool<T> where T : class, IPoolable<T> 
    {
        public event Action<T> OnObjectGet;

        private readonly Action<T> OnGet;
        private readonly Action<T> OnReturn;

        private readonly bool IsDynamic;
        private readonly int MaxPoolSize;
        private readonly int MinPoolSize;
        private readonly Queue<T> _objectPool;
        private readonly List<T> _aliveObjects;

        private readonly IFactory<T> _factory;

        private int NumberOfObjectInPool => _objectPool.Count;
        
        public ObjectPool(IFactory<T> factory,Action<T> onGet, Action<T> onReturn)
        {
            OnGet = onGet;
            OnReturn = onReturn;
            _aliveObjects  = new List<T>();
            _objectPool = new Queue<T>();

            _factory = factory;
        }
        
        public ObjectPool(IFactory<T> factory)
        {
            OnGet = null;
            OnReturn = null;
            _aliveObjects  = new List<T>();
            _objectPool = new Queue<T>();

            _factory = factory;
        }
        
        public ObjectPool(IFactory<T> factory,int initialPool)
        {
            OnGet = null;
            OnReturn = null;
            _aliveObjects  = new List<T>();
            _objectPool = new Queue<T>(initialPool);
            
            IsDynamic = true;
            
            _factory = factory;

            for (var i = 0; i < initialPool; i++)
            {
                var newObject = _factory.Create();
                _objectPool.Enqueue(newObject);
            }
        }

        public ObjectPool(IFactory<T> factory,Action<T> onGet, Action<T> onReturn, int maxPoolSize, int minPoolSize) : this(factory,onGet,onReturn)
        {
            IsDynamic = true;
            MaxPoolSize = maxPoolSize;
            MinPoolSize = minPoolSize;
        }
        
        public ObjectPool(IFactory<T> factory,Action<T> onGet, Action<T> onReturn, bool isDynamic,int initialPool) : this(factory,onGet,onReturn)
        {
            IsDynamic = isDynamic;
            MaxPoolSize = int.MaxValue;
            MinPoolSize = 0;
        }

        public ObjectPool(IFactory<T> factory,int maxPoolSize, int minPoolSize) : this(factory)
        {
            IsDynamic = true;
            MaxPoolSize = maxPoolSize;
            MinPoolSize = minPoolSize;
        }
        
        public ObjectPool(IFactory<T> factory,bool isDynamic,int initialPool) : this(factory,initialPool)
        {
            IsDynamic = isDynamic;
            MaxPoolSize = int.MaxValue;
            MinPoolSize = 0;
        }
        
        public ObjectPool(IFactory<T> factory,int minPoolSize,int initialPool,int maxPoolSize) : this(factory,initialPool)
        {
            IsDynamic = true;
            MaxPoolSize = maxPoolSize;
            MinPoolSize = minPoolSize;
        }


        private bool TryGetObjectFromPool(out T t)
        {
            if (_objectPool.Count > 0)
            {
                t = _objectPool.Dequeue();
                return true;
            }

            t = null;
            return false;
        }
        
        public T GetObject()
        {
            if (TryGetObjectFromPool(out var newObject))
            {
                newObject.OnDispose += Return;
                OnGet?.Invoke(newObject);
                OnObjectGet?.Invoke(newObject);
                return newObject;
            }

            if (!IsDynamic && NumberOfObjectInPool >= MaxPoolSize) return null;

            newObject = _factory.Create();
            newObject.OnDispose += Return;
            OnGet?.Invoke(newObject);
            AddObject(newObject);
            OnObjectGet?.Invoke(newObject);
            return newObject;
        }

        private void Return(T t)
        {
            OnReturn?.Invoke(t);
            ReturnObject(t);
        }

        private void AddObject(T t)
        {
            _aliveObjects.Add(t);
        }

        private void ReturnObject(T t)
        {
            _aliveObjects.Remove(t);
            _objectPool.Enqueue(t);
        }
    }
}