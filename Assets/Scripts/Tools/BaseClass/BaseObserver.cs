using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Tools.BaseClass
{
    public class BaseObserver<T>
    {
        protected static Dictionary<int, List<T>> ObserverObjects = new Dictionary<int, List<T>>();

        public static void AddObserverObject(T obj,int groupIndex = 0)
        {
            if (!ObserverObjects.TryGetValue(groupIndex, out var list))
            {
                list = new List<T>();
                ObserverObjects.Add(groupIndex, list);
            }   
            
            if (list.Contains(obj))
                return;
            
            list.Add(obj);
        }
        
        public static void RemoveObserverObject(T obj)
        {
            foreach (var objectsValue in ObserverObjects.Values)
            {
                if (objectsValue.Contains(obj))
                    objectsValue.Remove(obj);
            }
        }
    }
    
    public class BaseMonoObserver<T> : MonoBehaviour
    {
        protected static Dictionary<int, List<T>> ObserverObjects = new Dictionary<int, List<T>>();

        public static void AddObserverObject(T obj,int groupIndex = 0)
        {
            if (!ObserverObjects.TryGetValue(groupIndex, out var list))
            {
                list = new List<T>();
                ObserverObjects.Add(groupIndex, list);
            }   
            
            if (list.Contains(obj))
                return;
            
            list.Add(obj);
        }
        
        public static void RemoveObserverObject(T obj)
        {
            foreach (var objectsValue in ObserverObjects.Values)
            {
                if (objectsValue.Contains(obj))
                    objectsValue.Remove(obj);
            }
        }
    }
}