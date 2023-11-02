using Tzipory.ConfigFiles.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructureHandler : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private ProximityCircleManager _proximityCircleManager;
        [SerializeField] private PowerStructureConfig _powerStructureConfig;
        [SerializeField] private SpriteRenderer _powerStructureSpriteRenderer;

        
        private void Awake()
        {
            _proximityCircleManager.Init(id, _powerStructureConfig);
            if (_powerStructureConfig.PowerStructureSprite is null)
            {
                Debug.LogError("Config Sprite is missing");
                return;
            }
            _powerStructureSpriteRenderer.sprite = _powerStructureConfig.PowerStructureSprite;
        }

        private void OnValidate()
        {
            if (_powerStructureConfig.RingsRatios.Length != _proximityCircleManager.RingHandlers.Length)
            {
                Debug.LogError("the number of Rings in the SO is different than the actual rings in the prefab");
            }
        }
        
    }
}