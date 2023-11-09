using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityRingsManager : MonoBehaviour
    {
        public ProximityRingHandler[] RingHandlers => _ringHandlers;
        [SerializeField] private ProximityRingHandler[] _ringHandlers;
        [SerializeField] private ClickHelper _clickHelper;
        private bool _lockSpriteToggle;
        private Color _defaultColor;
        private Color _powerStructureTypeColor;
        private bool _shamanSelected;
        private float _ringDefaultSpriteAlpha;
        private float _ringSpriteAlphaFade;

        private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;


        public void Init(PowerStructureConfig powerStructureConfig)
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Init(i);
            }
            
            _ringDefaultSpriteAlpha = powerStructureConfig.DefaultSpriteAlpha;
            _ringSpriteAlphaFade = powerStructureConfig.SpriteAlphaFade;
            _defaultColor = powerStructureConfig.RingDefaultColor;
            _powerStructureTypeColor = powerStructureConfig.PowerStructureTypeColor;

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
        }
        private void ChangeAllRingsColors(Color color)
        {
            var ringSpriteAlpha = _ringDefaultSpriteAlpha;
            foreach (var ring in _ringHandlers)
            {
                color.a = ringSpriteAlpha;
                ring.ChangeColor(color);
                ringSpriteAlpha -= _ringSpriteAlphaFade;
            }
        }
        private void ScaleCircles(float circleRange, float[] ringsRanges)
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Scale(circleRange * ringsRanges[i]);
            }
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
        public void ToggleAllSprites(bool state)
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ToggleSprite(state);
            }
        }
        public void ToggleRingSprite(int ringId, bool state)
        {
            _ringHandlers[ringId].ToggleSprite(state);
        }
        private void ActivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleOuterRingSprite(true);
        }
        private void DeactivateRingSprites()
        {
            if (_shamanSelected) return;
            ToggleAllSprites(false);
        }
        private void ToggleOuterRingSprite(bool state)
        {
            _ringHandlers[^1].ToggleSprite(state);
        }

        #endregion
    }
}