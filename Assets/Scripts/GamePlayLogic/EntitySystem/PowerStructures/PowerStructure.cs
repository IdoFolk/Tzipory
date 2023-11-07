using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Systems.Entity;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructure : BaseGameEntity
    {
        [Header("Config File")] [SerializeField]
        private PowerStructureConfig _powerStructureConfig;

        [Header("Serialized Fields")] [SerializeField]
        private ProximityRingsManager proximityRingsManager;

        [SerializeField] private SpriteRenderer _powerStructureSpriteRenderer;
        [Space] [SerializeField] private bool _testing;

        private Dictionary<int, IDisposable> _activeStatusEffectOnShamans;
        private int _currentActiveRingId = 4;
        public void Init()
        {
            _activeStatusEffectOnShamans = new Dictionary<int, IDisposable>();

            if (_powerStructureConfig.PowerStructureSprite is null)
            {
                Debug.LogError("Config Sprite is missing");
                return;
            }

            proximityRingsManager.Init(_powerStructureConfig);
            _powerStructureSpriteRenderer.sprite = _powerStructureConfig.PowerStructureSprite;

            foreach (var ring in proximityRingsManager.RingHandlers)
            {
                ring.OnShamanEnter += OnShamanRingEnter;
                ring.OnShamanExit += OnShamanRingExit;
                ring.OnShadowEnter += OnShadowShamanEnter;
                ring.OnShadowExit += OnShadowShamanExit;
            }
        }

        private void OnValidate()
        {
            _powerStructureSpriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
            proximityRingsManager ??= GetComponentInChildren<ProximityRingsManager>();

            if (_powerStructureConfig is null) return;
            
            _powerStructureConfig.StatEffectConfig.StatModifier.ToggleRingModifiers(true);
            
            if (_powerStructureConfig.RingsRanges.Length != proximityRingsManager.RingHandlers.Length)
            {
                Debug.LogError("the number of Rings in the SO is different than the actual rings in the prefab");
            }
            
            _powerStructureSpriteRenderer.sprite = _powerStructureConfig.PowerStructureSprite;
        }

        private void OnDestroy()
        {
            foreach (var ring in proximityRingsManager.RingHandlers)
            {
                ring.OnShamanEnter -= OnShamanRingEnter;
                ring.OnShamanExit -= OnShamanRingExit;
                ring.OnShadowEnter -= OnShadowShamanEnter;
                ring.OnShadowExit -= OnShadowShamanExit;
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


            if (ringId == proximityRingsManager.RingHandlers.Length - 1)
            {
                if (!_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out IDisposable currentActiveStatusEffect)) return;
                currentActiveStatusEffect.Dispose();
                _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
            }
            else if (ringId < proximityRingsManager.RingHandlers.Length - 1)
            {
                if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out IDisposable currentActiveStatusEffect))
                {
                    currentActiveStatusEffect.Dispose();
                    _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
                }

                ringModifiedStatEffectConfig.StatModifier.Modifier = ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId + 1];
                IDisposable disposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                _activeStatusEffectOnShamans.Add(shaman.EntityInstanceID, disposable);
            }
        }

        private void OnShadowShamanEnter(int ringId)
        {
            if (_testing) Debug.Log($"Shadow Enter: {ringId}");

            if (ringId < _currentActiveRingId)
            {
                var currentActiveRing = proximityRingsManager.RingHandlers[ringId];

                if (_currentActiveRingId < proximityRingsManager.RingHandlers.Length)
                    proximityRingsManager.RingHandlers[_currentActiveRingId].ToggleSprite(false);
                currentActiveRing.ToggleSprite(true);
                _currentActiveRingId = currentActiveRing.Id;

                ShowStatPopupWindows(currentActiveRing);
            }
        }

        private void OnShadowShamanExit(int ringId)
        {
            if (_testing) Debug.Log($"Shadow Exit: {ringId}");

            if (ringId >= _currentActiveRingId)
            {
                var currentActiveRing = proximityRingsManager.RingHandlers[ringId];
                proximityRingsManager.ToggleAllSprites(false);
                _currentActiveRingId = currentActiveRing.Id + 1;

                if (_currentActiveRingId >= proximityRingsManager.RingHandlers.Length)
                    StatEffectPopupManager.HidePopupWindows(EntityInstanceID);
                else
                {
                    proximityRingsManager.ToggleRingSprite(_currentActiveRingId, true);
                    currentActiveRing = proximityRingsManager.RingHandlers[_currentActiveRingId];
                    ShowStatPopupWindows(currentActiveRing);
                }
            }
            else
            {
                Debug.LogError("ring id could not be found");
            }
        }

        private void ShowStatPopupWindows(ProximityRingHandler ringHandler)
        {
            var modifiedStatEffectValue = ModifyStatEffectByRing(ringHandler);
            var modifiedStatEffectPrecent = CalculateStatPercent(modifiedStatEffectValue);
            var roundedValue = MathF.Round(modifiedStatEffectPrecent);
            var statEffectName = _powerStructureConfig.StatEffectConfig.AffectedStatType.ToString();
            
            Color color = _powerStructureConfig.PowerStructureTypeColor;
            float alpha = _powerStructureConfig.DefaultSpriteAlpha - _powerStructureConfig.SpriteAlphaFade * ringHandler.Id;
            color.a = alpha;
            bool isPercent = _powerStructureConfig.StatEffectConfig.StatModifier.StatusModifierType == StatusModifierType.Multiplication;


            StatEffectPopupManager.ShowPopupWindows(EntityInstanceID, statEffectName, roundedValue, isPercent, color);

            
        }

        private float ModifyStatEffectByRing(ProximityRingHandler ringHandler)
        {
            float statEffectModifiedValue = _powerStructureConfig.StatEffectConfig.StatModifier.RingModifiers[ringHandler.Id];

            return statEffectModifiedValue;
        }

        private float CalculateStatPercent(float modifiedStatValue)
        {
            float statPercent = 0;
            float unModifiedStatValue = _powerStructureConfig.StatEffectConfig.StatModifier.Modifier;
            switch (_powerStructureConfig.StatEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    statPercent = modifiedStatValue;
                    break;
                case StatusModifierType.Multiplication:
                    statPercent = (modifiedStatValue - 1) * 100;
                    break;
            }

            return statPercent;
        }
    }
}