using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityInitializationComponent<in T> where T : ScriptableObject
    {
        public void Initialize(T config);
    }
}