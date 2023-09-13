using UnityEngine;

namespace Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig.EntityVisualConfig
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