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
        [SerializeField] private ProximityRingHandler[] _ringHandlers;
        [SerializeField] private ClickHelper _clickHelper;

        private bool _lockSpriteToggle;
        private Color _defaultColor;
        private Color _activeColor;
        private int _currentActiveRingId;
        private bool _shamanSelected;

        private StatEffectConfig _statEffectConfig;
        private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;


        public void Init(int id, PowerStructureConfig powerStructureConfig)
        {
            Id = id;
            float ringSpriteAlpha = powerStructureConfig.DefaultSpriteAlpha;
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Init(i, ringSpriteAlpha);
                _ringHandlers[i].OnShadowEnter += OnShadowShamanEnter;
                _ringHandlers[i].OnShadowExit += OnShadowShamanExit;
                ringSpriteAlpha -= powerStructureConfig.SpriteAlphaFade;
            }

            _statEffectConfig = powerStructureConfig.StatEffectConfig;
            _defaultColor = powerStructureConfig.RingOnHoverColor;
            _activeColor = powerStructureConfig.RingOnShamanHoverColor;
            _currentActiveRingId = _ringHandlers.Length;

            _clickHelper.OnEnterHover += ActivateRingSprites;
            _clickHelper.OnExitHover += DeactivateRingSprites;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected += OnShamanSelect;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected += OnShamanDeselect;
            ScaleCircles(powerStructureConfig.Range, powerStructureConfig.RingsRatios);
            ChangeAllRingsColors(_defaultColor);
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
            if (ringId < _currentActiveRingId)
            {
                var currentActiveRing = _ringHandlers[ringId];
                EnterActiveSprite(currentActiveRing);
                ShowStatPopupWindows(currentActiveRing);
            }
        }

        private void OnShadowShamanExit(int ringId)
        {
            if (ringId == _currentActiveRingId)
            {
                var currentActiveRing = _ringHandlers[ringId];
                ExitActiveSprite(currentActiveRing);
                if (_currentActiveRingId == _ringHandlers.Length)
                {
                    HideStatPopupWindow();
                }
                else
                {
                    currentActiveRing = _ringHandlers[_currentActiveRingId];
                    ShowStatPopupWindows(currentActiveRing);
                }
            }
            else
            {
                Debug.LogError("ring detect Problem");
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
            var statEffectName = _statEffectConfig.AffectedStatType.ToString();
            StatBonusPopupManager.ShowPopupWindows(Id,ringHandler.Id,statEffectName, modifiedStatEffectPrecent);
        }

        private void HideStatPopupWindow()
        {
            var statEffectName = _statEffectConfig.AffectedStatType.ToString();
            StatBonusPopupManager.HidePopupWindows(Id);
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
            float statEffectModifiedValue = 0;
            float ringPercentage;
            float modifier = _statEffectConfig.StatModifier.Modifier;
            switch (_statEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    //ringPercentage = modifier / _ringHandlers.Length;
                    //statEffectModifiedValue = modifier - ringPercentage * ringHandler.Id;
                    break;
                case StatusModifierType.Multiplication:
                    ringPercentage = modifier / _ringHandlers.Length;
                    statEffectModifiedValue = modifier - ringPercentage * ringHandler.Id;
                    break;
            }

            return statEffectModifiedValue;
        }

        private float CalculateStatPercent(float modifiedStatValue)
        {
            float statPercent = 0;
            switch (_statEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    //turn into prectentage
                    break;
            }

            return modifiedStatValue; //change
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
            if (_currentActiveRingId != _ringHandlers.Length)
                _ringHandlers[_currentActiveRingId].ToggleSprite(false);
            ring.ToggleSprite(true);
            _currentActiveRingId = ring.Id;
        }
        private void ExitActiveSprite(ProximityRingHandler ring)
        {
            ring.ToggleSprite(false);
            _currentActiveRingId += 1;
            if (_currentActiveRingId == _ringHandlers.Length) return;
            _ringHandlers[_currentActiveRingId].ToggleSprite(true);
        }
        private void ActivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleActiveSprite(true);
        }
        private void DeactivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleActiveSprite(false);
        }
        private void ToggleActiveSprite(bool state)
        {
            _ringHandlers[^1].ToggleSprite(state);
        }

        #endregion
    }
}