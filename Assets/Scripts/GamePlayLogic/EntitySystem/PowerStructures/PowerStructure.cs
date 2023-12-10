using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.Helpers.Consts;
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

        private Dictionary<int, IStatEffectProcess> _activeStatusEffectOnShamans;
        private int _currentActiveRingId = 4;

        public void Init()
        {
            _activeStatusEffectOnShamans = new Dictionary<int, IStatEffectProcess>();

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
            {
                currentActiveStatusEffect.Dispose();
                IStatEffectProcess shamanDisposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                _activeStatusEffectOnShamans[shaman.EntityInstanceID] = shamanDisposable;
            }
            else
            {
                IStatEffectProcess shamanDisposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                _activeStatusEffectOnShamans.Add(shaman.EntityInstanceID, shamanDisposable);
            }
        }

        private void OnShamanRingExit(int ringId, Shaman shaman)
        {
            Logger.Log($"shaman exited ring {ringId}", POWER_STRUCTURE_LOG_GROUP);
            var ringModifiedStatEffectConfig = _powerStructureConfig.StatEffectConfig;


            if (ringId == proximityRingsManager.RingHandlers.Length - 1)
            {
                if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out var currentActiveStatusEffect))
                {
                    currentActiveStatusEffect.Dispose();
                    _activeStatusEffectOnShamans.Remove(shaman.EntityInstanceID);
                }
            }
            else if (ringId < proximityRingsManager.RingHandlers.Length - 1)
            {
                ringModifiedStatEffectConfig.StatModifier.Modifier =
                    ringModifiedStatEffectConfig.StatModifier.RingModifiers[ringId + 1];
                if (_activeStatusEffectOnShamans.TryGetValue(shaman.EntityInstanceID, out var currentActiveStatusEffect))
                {
                    currentActiveStatusEffect.Dispose();
                    IStatEffectProcess shamanDisposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                    _activeStatusEffectOnShamans[shaman.EntityInstanceID] = shamanDisposable;
                }
                else
                {
                    IStatEffectProcess disposable = shaman.StatHandler.AddStatEffect(ringModifiedStatEffectConfig);
                    _activeStatusEffectOnShamans.Add(shaman.EntityInstanceID, disposable);
                }
            }
        }

        private void OnShadowShamanEnter(int ringId, Shaman shaman)
        {
            if (_testing) Logger.Log($"Shadow Enter: {ringId}", POWER_STRUCTURE_LOG_GROUP);

            if (ringId < _currentActiveRingId)
            {
                var currentActiveRing = proximityRingsManager.RingHandlers[ringId];

                if (_currentActiveRingId < proximityRingsManager.RingHandlers.Length)
                    proximityRingsManager.RingHandlers[_currentActiveRingId].ToggleSprite(false);
                currentActiveRing.ToggleSprite(true);
                _currentActiveRingId = currentActiveRing.Id;

                ShowStatPopupWindows(currentActiveRing, shaman);
            }
        }

        private void OnShadowShamanExit(int ringId, Shaman shaman)
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
                HideStatPopupWindows(shaman);
            }
            else
            {
                proximityRingsManager.ToggleRingSprite(_currentActiveRingId, true);
                currentActiveRing = proximityRingsManager.RingHandlers[_currentActiveRingId];
                ShowStatPopupWindows(currentActiveRing, shaman);
            }
        }

        private void ShowStatPopupWindows(ProximityRingHandler ringHandler, Shaman shaman)
        {
            Color color = _powerStructureConfig.PowerStructureTypeColor;
            float alpha = _powerStructureConfig.DefaultSpriteAlpha -
                          _powerStructureConfig.SpriteAlphaFade * ringHandler.Id;
            color.a = alpha;
            if (shaman.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType, out var shamanStat))
            {
                bool isPercent = _powerStructureConfig.StatEffectConfig.StatModifier.StatusModifierType == StatusModifierType.Multiplication;
                
                var statEffectValue = GetStatEffectByRing(ringHandler);
                var modifiedStatEffect = ModifyStatEffectForDisplay(shamanStat,statEffectValue, true);
                StatEffectPopupManager.ShowPopupWindows(EntityInstanceID, shamanStat, modifiedStatEffect, true, color);

                var changedValue = GetDeltaFromStatEffectOnShaman(statEffectValue, shamanStat, true);
                HeroSelectionUI.Instance.UpdateStatBlocks(shamanStat, changedValue);
            }
        }

        private void HideStatPopupWindows(Shaman shaman)
        {
            StatEffectPopupManager.HidePopupWindows(EntityInstanceID);

            if (shaman.Stats.TryGetValue((int)_powerStructureConfig.StatEffectConfig.AffectedStatType, out var shamanStat))
            {
                var changedValue = shamanStat.BaseValue - shamanStat.CurrentValue;
                var roundedValue = Mathf.Round(changedValue);
                HeroSelectionUI.Instance.UpdateStatBlocks(shamanStat, roundedValue);
            }
        }

        private float GetStatEffectByRing(ProximityRingHandler ringHandler)
        {
            float statEffectModifiedValue =
                _powerStructureConfig.StatEffectConfig.StatModifier.RingModifiers[ringHandler.Id];

            return statEffectModifiedValue;
        }

        private float ModifyStatEffectForDisplay(Stat shamanStat,float modifiedStatValue, bool rounded)
        {
            float statValue = 0;
            float statPercent = 0;
            switch ((Constant.StatsId)shamanStat.Id)
            {
                case Constant.StatsId.CritChance:
                    statPercent = (modifiedStatValue * shamanStat.BaseValue) / shamanStat.BaseValue;
                    statValue = (statPercent - 1) * 100;
                    break;
                case Constant.StatsId.AttackDamage:
                    statPercent = (modifiedStatValue * shamanStat.BaseValue) / shamanStat.BaseValue;
                    statValue = (statPercent - 1) * 100;
                    break;
                case Constant.StatsId.CritDamage:
                    statPercent = (modifiedStatValue * shamanStat.BaseValue) / shamanStat.BaseValue;
                    statValue = (statPercent - 1) * 100;
                    break;
            }
            

            if (!rounded) return statValue;
            float roundedValue = MathF.Round(statValue);
            return roundedValue;
        }

        private float GetDeltaFromStatEffectOnShaman(float StatEffectValue, Stat shamanStat, bool rounded)
        {
            float value = StatEffectValue;
            switch (_powerStructureConfig.StatEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    break;
                case StatusModifierType.Reduce:
                    break;
                case StatusModifierType.Multiplication:
                    value = (shamanStat.BaseValue * StatEffectValue) - shamanStat.CurrentValue;
                    break;
            }

            if (!rounded) return value;
            float roundedValue = MathF.Round(value);
            return roundedValue;
        }
    }
}