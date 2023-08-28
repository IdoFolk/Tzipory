using Tzipory.Systems.FactorySystem;
using UnityEngine;

namespace Systems.FactorySystem
{
    public abstract class BaseGameObjectFactory<T> : IFactory<T> where T : MonoBehaviour 
    {
        protected abstract string GameObjectPath { get; }
        
        private readonly T _monoBehaviour;

        protected BaseGameObjectFactory()
        {
            _monoBehaviour = Resources.Load<T>(GameObjectPath);

            if (_monoBehaviour == null)
                throw new System.Exception($"Monobehavir not found in path {GameObjectPath}");
        }
        
        public T Create()
        {
            var monoBehaviour =  Object.Instantiate(_monoBehaviour);
            monoBehaviour.gameObject.SetActive(false);
            return monoBehaviour;
        }
    }
}