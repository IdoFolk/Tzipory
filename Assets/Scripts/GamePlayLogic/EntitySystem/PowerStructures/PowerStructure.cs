using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructure : MonoBehaviour
    {
        private static int _IdCount = 0;
        
        [SerializeField] private int _id;
        [SerializeField] private ProximityCircleManager _proximityCircleManager;
        [SerializeField] private PowerStructureConfig _powerStructureConfig;
        [SerializeField] private SpriteRenderer _powerStructureSpriteRenderer;
        [SerializeField] private bool _testing;
        
        private Dictionary<int, IDisposable> _activeStatusEffectOnShamans;

        
        private void Awake()
        {
            _id = _IdCount;
            _IdCount++;
            _activeStatusEffectOnShamans = new Dictionary<int, IDisposable>();

            _proximityCircleManager.Init(_id, _powerStructureConfig, _testing);
            if (_powerStructureConfig.PowerStructureSprite is null)
            {
                Debug.LogError("Config Sprite is missing");
                return;
            }
            _powerStructureSpriteRenderer.sprite = _powerStructureConfig.PowerStructureSprite;

            foreach (var ring in _proximityCircleManager.RingHandlers)
            {
                ring.OnShamanEnter += OnShamanRingEnter;
                ring.OnShamanExit += OnShamanRingExit;
            }
        }

        private void OnValidate()
        {
            if (_powerStructureConfig.RingsRanges.Length != _proximityCircleManager.RingHandlers.Length)
            {
                Debug.LogError("the number of Rings in the SO is different than the actual rings in the prefab");
            }
        }

        private void OnDestroy()
        {
            foreach (var ring in _proximityCircleManager.RingHandlers)
            {
                ring.OnShamanEnter -= OnShamanRingEnter;
                ring.OnShamanExit -= OnShamanRingExit;
            }
        }

        private void OnShamanRingEnter(int ringId, Shaman shaman)
        {
            Debug.Log($"shaman entered ring {ringId}");

            var ringModifiedStatEffectConfig = _powerStructureConfig.StatEffectConfig;
            ringModifiedStatEffectConfig.StatModifier.Modifier = ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId];

            if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out var currentActiveStatusEffect))
                currentActiveStatusEffect.Dispose();
            
            IDisposable shamanDisposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
            
            _activeStatusEffectOnShamans[shaman.EntityInstanceID] = shamanDisposable;
        }
        private void OnShamanRingExit(int ringId, Shaman shaman)
        {
            Debug.Log($"shaman exited ring {ringId}");
            var ringModifiedStatEffectConfig = _powerStructureConfig.StatEffectConfig;
            
            
            if (ringId == _proximityCircleManager.RingHandlers.Length-1)
            {
                if (!_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out IDisposable currentActiveStatusEffect)) return;
                currentActiveStatusEffect.Dispose();
                _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
            }
            else if (ringId < _proximityCircleManager.RingHandlers.Length - 1)
            {
                if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID,out IDisposable currentActiveStatusEffect))
                {
                    currentActiveStatusEffect.Dispose();
                    _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
                }
                
                ringModifiedStatEffectConfig.StatModifier.Modifier = ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId+1];
                IDisposable disposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                _activeStatusEffectOnShamans.Add(shaman.
                    EntityInstanceID, disposable);
            }
        }
    }
}