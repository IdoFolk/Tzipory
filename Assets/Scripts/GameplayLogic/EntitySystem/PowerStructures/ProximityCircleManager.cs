using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityCircleManager : MonoBehaviour
    {
        [HideInInspector]public int Id { get; private set; }
        public ProximityRingHandler[] RingHandlers => _ringHandlers;
        public Color PowerStructureTypeColor => _powerStructureTypeColor;
        [SerializeField] private ProximityRingHandler[] _ringHandlers;
        [SerializeField] private ClickHelper _clickHelper;
        private PowerStructureConfig _powerStructureConfig;
        private bool _lockSpriteToggle;
        private Color _defaultColor;
        private Color _powerStructureTypeColor;
        private int _currentActiveRingId;
        private bool _shamanSelected;
        private bool _testing;

        private StatEffectConfig _statEffectConfig;
        private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;


        public void Init(int id, PowerStructureConfig powerStructureConfig, bool testing)
        {
            Id = id;
            _testing = testing;
            float ringSpriteAlpha = powerStructureConfig.DefaultSpriteAlpha;
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Init(i, ringSpriteAlpha);
                _ringHandlers[i].OnShadowEnter += OnShadowShamanEnter;
                _ringHandlers[i].OnShadowExit += OnShadowShamanExit;
                ringSpriteAlpha -= powerStructureConfig.SpriteAlphaFade;
            }

            _powerStructureConfig = powerStructureConfig;
            
            _statEffectConfig = powerStructureConfig.StatEffectConfig;
            _defaultColor = powerStructureConfig.RingDefaultColor;
            _powerStructureTypeColor = powerStructureConfig.PowerStructureTypeColor;
            _currentActiveRingId = _ringHandlers.Length;

            _clickHelper.OnEnterHover += ActivateRingSprites;
            _clickHelper.OnExitHover += DeactivateRingSprites;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected += OnShamanSelect;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected += OnShamanDeselect;
            ScaleCircles(powerStructureConfig.Range, powerStructureConfig.RingsRanges);
            ChangeAllRingsColors(_powerStructureTypeColor);
        }

        private void OnDestroy()
        {
            _clickHelper.OnEnterHover -= ActivateRingSprites;
            _clickHelper.OnExitHover -= DeactivateRingSprites;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected -= OnShamanSelect;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected -= OnShamanDeselect;
            foreach (var ringHandler in _ringHandlers)
            {
                ringHandler.OnShadowEnter -= OnShadowShamanEnter;
                ringHandler.OnShadowExit -= OnShadowShamanExit;
            }
        }

        private void OnShadowShamanEnter(int ringId)
        {
            if (_testing) Debug.Log($"Shadow Enter: {ringId}");
            
            if (ringId < _currentActiveRingId)
            {
                var currentActiveRing = _ringHandlers[ringId];
                EnterActiveSprite(currentActiveRing);
                ShowStatPopupWindows(currentActiveRing);
            }
        }

        private void OnShadowShamanExit(int ringId)
        {
            if (_testing) Debug.Log($"Shadow Exit: {ringId}");
            
            if (ringId >= _currentActiveRingId)
            {
                var currentActiveRing = _ringHandlers[ringId];
                ExitActiveSprite(currentActiveRing);
                if (_currentActiveRingId >= _ringHandlers.Length) StatBonusPopupManager.HidePopupWindows(Id);
                else
                {
                    currentActiveRing = _ringHandlers[_currentActiveRingId];
                    ShowStatPopupWindows(currentActiveRing);
                }
            }
            else
            {
                Debug.LogError("ring id could not be found");
            }
        }


        private void ChangeAllRingsColors(Color color)
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ChangeColor(color);
            }
        }

        private void ShowStatPopupWindows(ProximityRingHandler ringHandler)
        {
            var modifiedStatEffectValue = ModifyStatEffectByRing(ringHandler);
            var modifiedStatEffectPrecent = CalculateStatPercent(modifiedStatEffectValue);
            var roundedValue = MathF.Round(modifiedStatEffectPrecent);
            var statEffectName = _statEffectConfig.AffectedStatType.ToString();
            StatBonusPopupManager.ShowPopupWindows(this,ringHandler.Id,statEffectName, roundedValue);
        }

        private void ScaleCircles(float circleRange, float[] ringsRanges)
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Scale(circleRange * ringsRanges[i]);
            }
        }

        private float ModifyStatEffectByRing(ProximityRingHandler ringHandler)
        {
            float statEffectModifiedValue = _statEffectConfig.StatModifier.RingModifiers[ringHandler.Id];

            return statEffectModifiedValue;
        }

        private float CalculateStatPercent(float modifiedStatValue)
        {
            float statPercent = 0;
            float unModifiedStatValue = _statEffectConfig.StatModifier.Modifier;
            switch (_statEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    var modifiedRatio = modifiedStatValue / unModifiedStatValue;
                    statPercent = (modifiedRatio - 1) * 100;
                    break;
                case StatusModifierType.Multiplication:
                    statPercent = (modifiedStatValue - 1) * 100;
                    break;
            }

            return statPercent; 
        }
        private void OnShamanSelect()
        {
            _shamanSelected = true;
        }
        private void OnShamanDeselect()
        {
            _shamanSelected = false;
        }
        
        #region SpriteActivationToggle
        private void EnterActiveSprite(ProximityRingHandler ring)
        {
            if (_currentActiveRingId < _ringHandlers.Length)
                _ringHandlers[_currentActiveRingId].ToggleSprite(false);
            ring.ToggleSprite(true);
            _currentActiveRingId = ring.Id;
        }
        private void ExitActiveSprite(ProximityRingHandler ring)
        {
            ToggleAllSprites(false);
            _currentActiveRingId = ring.Id + 1;
            if (_currentActiveRingId >= _ringHandlers.Length) return;
            _ringHandlers[_currentActiveRingId].ToggleSprite(true);
        }

        private void ToggleAllSprites(bool state)
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ToggleSprite(state);
            }
        }
        private void ActivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleActiveSprite(true);
        }
        private void DeactivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleAllSprites(false);
        }
        private void ToggleActiveSprite(bool state)
        {
            _ringHandlers[^1].ToggleSprite(state);
        }

        #endregion
    }
}