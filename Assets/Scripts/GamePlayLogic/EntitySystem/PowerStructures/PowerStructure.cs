using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Systems.Entity;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class PowerStructure : BaseGameEntity
    {
        private const string POWER_STRUCTURE_LOG_GROUP = "PowerStructure";

        [Header("Config File")] [SerializeField]
        private PowerStructureConfig _powerStructureConfig;

        [Header("Serialized Fields")] [SerializeField]
        private ProximityRingsManager proximityRingsManager;

        [SerializeField] private SpriteRenderer _powerStructureSpriteRenderer;
        [Space] [SerializeField] private bool _testing;

        private Dictionary<int, IDisposable> _activeStatusEffectOnShamans;
        private IDisposable _activeStatusEffectOnShadow;
        private int _currentActiveRingId = 4;

        public void Init()
        {
            _activeStatusEffectOnShamans = new Dictionary<int, IDisposable>();

            if (_powerStructureConfig.PowerStructureSprite is null)
            {
                Logger.LogError("Config Sprite is missing");
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
                Logger.LogError("the number of Rings in the SO is different than the actual rings in the prefab");
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
            Logger.Log($"shaman entered ring {ringId}", POWER_STRUCTURE_LOG_GROUP);

            var ringModifiedStatEffectConfig = _powerStructureConfig.StatEffectConfig;
            ringModifiedStatEffectConfig.StatModifier.Modifier =
                ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId];

            if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out var currentActiveStatusEffect))
                currentActiveStatusEffect.Dispose();

            IDisposable shamanDisposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);

            _activeStatusEffectOnShamans[shaman.EntityInstanceID] = shamanDisposable;
        }

        private void OnShamanRingExit(int ringId, Shaman shaman)
        {
            Logger.Log($"shaman exited ring {ringId}", POWER_STRUCTURE_LOG_GROUP);
            var ringModifiedStatEffectConfig = _powerStructureConfig.StatEffectConfig;


            if (ringId == proximityRingsManager.RingHandlers.Length - 1)
            {
                if (!_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID,
                        out IDisposable currentActiveStatusEffect)) return;
                currentActiveStatusEffect.Dispose();
                _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
            }
            else if (ringId < proximityRingsManager.RingHandlers.Length - 1)
            {
                if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID,
                        out IDisposable currentActiveStatusEffect))
                {
                    currentActiveStatusEffect.Dispose();
                    _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
                }

                ringModifiedStatEffectConfig.StatModifier.Modifier =
                    ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId + 1];
                IDisposable disposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                _activeStatusEffectOnShamans.Add(shaman.EntityInstanceID, disposable);
            }
        }

        private void OnShadowShamanEnter(int ringId, Shaman shaman, Shadow shadow)
        {
            if (_testing) Logger.Log($"Shadow Enter: {ringId}", POWER_STRUCTURE_LOG_GROUP);

            if (ringId < _currentActiveRingId)
            {
                var currentActiveRing = proximityRingsManager.RingHandlers[ringId];

                if (_currentActiveRingId < proximityRingsManager.RingHandlers.Length)
                    proximityRingsManager.RingHandlers[_currentActiveRingId].ToggleSprite(false);
                currentActiveRing.ToggleSprite(true);
                _currentActiveRingId = currentActiveRing.Id;

                if (_activeStatusEffectOnShadow is not null)
                {
                    _activeStatusEffectOnShadow.Dispose();
                    _activeStatusEffectOnShadow = null;
                }
                var statEffectConfig = _powerStructureConfig.StatEffectConfig;
                statEffectConfig.StatModifier.Modifier = _powerStructureConfig.StatEffectConfig.StatModifier.RingModifiers[ringId];
                IDisposable disposable = shadow.StatHandler.AddStatEffect(statEffectConfig);
                _activeStatusEffectOnShadow = disposable;

                ShowStatPopupWindows(currentActiveRing, shaman, shadow);
            }
        }

        private void OnShadowShamanExit(int ringId, Shaman shaman, Shadow shadow)
        {
            if (_testing) Logger.Log($"Shadow Exit: {ringId}", POWER_STRUCTURE_LOG_GROUP);

            if (ringId < _currentActiveRingId) return;
            var currentActiveRing = proximityRingsManager.RingHandlers[ringId];
            proximityRingsManager.ToggleAllSprites(false);
            _currentActiveRingId = currentActiveRing.Id + 1;
            if (_currentActiveRingId > proximityRingsManager.RingHandlers.Length)
                _currentActiveRingId = proximityRingsManager.RingHandlers.Length;

            if (_currentActiveRingId >= proximityRingsManager.RingHandlers.Length)
            {
                if (_activeStatusEffectOnShadow is not null)
                {
                    _activeStatusEffectOnShadow.Dispose();
                    _activeStatusEffectOnShadow = null;
                }
                HideStatPopupWindows(shaman, shadow);
            }
            else
            {
                if (_activeStatusEffectOnShadow is not null)
                {
                    _activeStatusEffectOnShadow.Dispose();
                    _activeStatusEffectOnShadow = null;
                }
                var statEffectConfig = _powerStructureConfig.StatEffectConfig;
                statEffectConfig.StatModifier.Modifier = _powerStructureConfig.StatEffectConfig.StatModifier.RingModifiers[ringId];
                IDisposable disposable = shadow.StatHandler.AddStatEffect(statEffectConfig);
                _activeStatusEffectOnShadow = disposable;
                
                proximityRingsManager.ToggleRingSprite(_currentActiveRingId, true);
                currentActiveRing = proximityRingsManager.RingHandlers[_currentActiveRingId];
                ShowStatPopupWindows(currentActiveRing, shaman, shadow);
            }
        }

        private void ShowStatPopupWindows(ProximityRingHandler ringHandler, Shaman shaman, Shadow shadow)
        {
            var modifiedStatEffectValue = ModifyStatEffectByRing(ringHandler);
            var modifiedStatEffectPrecent = CalculateStatPercent(modifiedStatEffectValue);
            var roundedValue = MathF.Round(modifiedStatEffectPrecent);

            Color color = _powerStructureConfig.PowerStructureTypeColor;
            float alpha = _powerStructureConfig.DefaultSpriteAlpha -
                          _powerStructureConfig.SpriteAlphaFade * ringHandler.Id;
            color.a = alpha;
            bool isPercent = _powerStructureConfig.StatEffectConfig.StatModifier.StatusModifierType ==
                             StatusModifierType.Multiplication;

            if (shaman.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType,
                    out var shamanStat))
            {
                StatEffectPopupManager.ShowPopupWindows(EntityInstanceID, shamanStat, roundedValue, isPercent, color);
                if (shadow.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType, out var shadowStat))
                {
                    HeroSelectionUI.Instance.UpdateSelectionUI(shamanStat, shadowStat);
                }
            }
        }

        private void HideStatPopupWindows(Shaman shaman, Shadow shadow)
        {
            StatEffectPopupManager.HidePopupWindows(EntityInstanceID);
            if (shaman.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType,
                    out var shamanStat))
            {
                if (shadow.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType, out var shadowStat))
                {
                    HeroSelectionUI.Instance.UpdateSelectionUI(shamanStat, shadowStat);
                }
            }
        }

        private float ModifyStatEffectByRing(ProximityRingHandler ringHandler)
        {
            float statEffectModifiedValue =
                _powerStructureConfig.StatEffectConfig.StatModifier.RingModifiers[ringHandler.Id];

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

    public class ActiveStatusEffect : IDisposable
    {
        //figure out what disposable means
        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}