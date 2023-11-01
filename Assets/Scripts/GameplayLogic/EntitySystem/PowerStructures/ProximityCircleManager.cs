using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Helpers;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Enums;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityCircleManager : MonoBehaviour, ITargetableReciever
    {
        public ProximityRingHandler[] RingHandlers => _ringHandlers;
        [SerializeField] private ProximityRingHandler[] _ringHandlers;
        [SerializeField] private ClickHelper _clickHelper;

        private bool _lockSpriteToggle;
        private Color _defaultColor;
        private Color _activeColor;

        private StatEffectConfig _statEffectConfig;
        private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;


        public void Init(PowerStructureConfig powerStructureConfig)
        {
            float ringSpriteAlpha = powerStructureConfig.DefaultSpriteAlpha;
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Init(i + 1, this, ringSpriteAlpha);
                ringSpriteAlpha -= powerStructureConfig.SpriteAlphaFade;
            }
            _statEffectConfig = powerStructureConfig.StatEffectConfig;
            _defaultColor = powerStructureConfig.RingOnHoverColor;
            _activeColor = powerStructureConfig.RingOnShamanHoverColor;
            ScaleCircles(powerStructureConfig.Range, powerStructureConfig.RingsRatios);
            ChangeAllRingsColors(powerStructureConfig.RingOnHoverColor);
            _clickHelper.OnEnterHover += ActivateRingSprites;
            _clickHelper.OnExitHover += DeactivateRingSprites;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected += ActivateRingSpritesWithLock;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected += DeactivateRingSpritesWithLock;
        }

        private void ChangeAllRingsColors(Color color)
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ChangeColor(color);
            }
        }

        private void ResetActiveRings()
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ActivateRing(false);
            }
        }

        private void ChangeCurrentProximityRing()
        {
            foreach (var ring in _ringHandlers)
            {
                if (!ring.ColliderTargetingArea.IsCollidingWithShadow) continue;
                ring.ActivateRing(true);
                ring.ChangeColor(_activeColor);
                break;
            }
        }

        private void ActivateStatPopupWindows()
        {
            var modifiedStatEffectValue = ModifyStatEffectByRing();
            var modifiedStatEffectPrecent = CalculateStatPercent(modifiedStatEffectValue);
            var statEffectName = _statEffectConfig.AffectedStatType.ToString();
            StatBonusPopupManager.ShowPopupWindows(statEffectName,modifiedStatEffectPrecent);
        }

        private void ScaleCircles(float circleRange, float[] ringsRanges)
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Scale(circleRange * ringsRanges[i]);
            }
        }

        private float ModifyStatEffectByRing()
        {
            float statEffectModifiedValue = 0;
            float ringPercentage;
            ProximityRingHandler activeRing;
            switch (_statEffectConfig.StatModifier.StatusModifierType)
            {
                case StatusModifierType.Addition:
                    ringPercentage = _statEffectConfig.StatModifier.Modifier / _ringHandlers.Length;
                    activeRing = FindActiveRing();
                    if (activeRing is null) Debug.LogError("Active Ring is null");
                    statEffectModifiedValue = ringPercentage * activeRing.Id;
                    break;
                case StatusModifierType.Multiplication:
                    ringPercentage = _statEffectConfig.StatModifier.Modifier / _ringHandlers.Length;
                    activeRing = FindActiveRing();
                    if (activeRing is null) Debug.LogError("Active Ring is null");
                    statEffectModifiedValue = ringPercentage * activeRing.Id;
                    break;
            }
            return statEffectModifiedValue;
        }

        private ProximityRingHandler FindActiveRing()
        {
            foreach (var ring in _ringHandlers)
            {
                if (ring.ActiveRing) return ring;
            }

            return null;
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

        #region SpriteActivationToggle

        private void ActivateRingSprites()
        {
            ToggleActiveSprite(true, false);
        }

        private void ActivateRingSpritesWithLock()
        {
            _lockSpriteToggle = true;
            ToggleActiveSprite(true, true);
        }

        private void DeactivateRingSprites()
        {
            ToggleActiveSprite(false, false);
        }

        private void DeactivateRingSpritesWithLock()
        {
            _lockSpriteToggle = false;
            ToggleActiveSprite(false, true);
        }

        private void ToggleActiveSprite(bool state, bool lockOverride)
        {
            if (!lockOverride)
                if (_lockSpriteToggle)
                    return;

            foreach (var ringHandler in _ringHandlers)
            {
                ringHandler.ToggleSprite(state);
            }
        }

        #endregion

        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            if (other.gameObject.CompareTag("ShadowShaman"))
            {
                for (var i = 0; i < _ringHandlers.Length; i++)
                {
                    ChangeAllRingsColors(_defaultColor);
                    ResetActiveRings();
                    ChangeCurrentProximityRing();

                    ActivateStatPopupWindows();
                }
            }
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            if (targetable is not Shaman shaman) return;

            if (_activeStatusEffectOnShaman.ContainsKey(shaman.EntityInstanceID)) //temp!!!
                return;

            //_statEffectConfig.StatModifier.ModifyStatEffect(ModifyStatEffectByRing());

            IDisposable disposable = shaman.StatHandler.AddStatEffect(_statEffectConfig); //need to modify the modifier
            _activeStatusEffectOnShaman.Add(shaman.EntityInstanceID, disposable);
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            if (targetable is not Shaman shaman) return;

            if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID, out IDisposable disposable))
            {
                disposable.Dispose();
                _activeStatusEffectOnShaman.Remove(shaman.EntityInstanceID);
            }
        }
    }
}