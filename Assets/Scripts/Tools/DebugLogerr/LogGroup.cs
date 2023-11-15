using Sirenix.OdinInspector;
using Tzipory.Systems.SaveLoadSystem;
using UnityEngine;

namespace Tzipory.Tools.Debag
{
    [System.Serializable]
    public class LogGroup : ISave
    {
        [SerializeField,HideInInspector] public string Name;
        [SerializeField] public bool IsActive;
        [SerializeField] public Color Color;
        public string ObjectName => Name;

        [Button]
        public void Delete()
        {
            Logger.DeleteGroup(this);
        }
    }
}