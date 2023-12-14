using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.EntityVisual
{
    [System.Serializable]
    public class BaseUnitEntityVisualConfig
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _icon;

        public Sprite Sprite => _sprite;        
        public string Name => _name;
        public Sprite Icon => _icon;
    }
}