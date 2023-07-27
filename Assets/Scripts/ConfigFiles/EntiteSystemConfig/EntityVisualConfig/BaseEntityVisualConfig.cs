using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig
{
    [System.Serializable]
    public class BaseEntityVisualConfig
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _icon;

        public Sprite Sprite => _sprite;

        public Sprite Icon => _icon;
    }
}