using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.EntityVisual
{
    [System.Serializable]
    public class BaseUnitEntityVisualConfig
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _icon;

        public Sprite Sprite => _sprite;

        public Sprite Icon => _icon;
    }
}