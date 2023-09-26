using System.Collections.Generic;

namespace Tzipory.Tools.BaseObserver
{
    public class BaseObserver<T>
    {
        protected static List<T> ObserverObjects = new List<T>();

        public static void AddObserverObject(T obj)
        {
            if (ObserverObjects.Contains(obj))
                return;
            ObserverObjects.Add(obj);
        }
        
        public static void RemoveObserverObject(T obj)
        {
            if (!ObserverObjects.Contains(obj))
                return;
            ObserverObjects.Remove(obj);
        }
    }
}