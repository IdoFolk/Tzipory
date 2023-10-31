using Tzipory.Helpers;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityCircleManager : MonoBehaviour
    {
        public ProximityRingHandler[] RingHandlers => _ringHandlers;
        [SerializeField] private ProximityRingHandler[] _ringHandlers;
        [SerializeField] private ClickHelper _clickHelper;
        
        private bool _lockSpriteToggle;
        private bool[] _ringIsColliding;
        private Color _defaultColor;
        private Color _activeColor;
        public void Init(PowerStructureConfig powerStructureConfig, ITargetableReciever reciever)
        {
            float ringSpriteAlpha = powerStructureConfig.DefaultSpriteAlpha;
            foreach (var ring in _ringHandlers)
            {
                ring.Init(reciever,ringSpriteAlpha);
                ringSpriteAlpha -= powerStructureConfig.SpriteAlphaFade;
            }
            
            _ringIsColliding = new bool[_ringHandlers.Length];
            _defaultColor = powerStructureConfig.RingOnHoverColor;
            _activeColor = powerStructureConfig.RingOnShamanHoverColor;
            ScaleCircles(powerStructureConfig.Range,powerStructureConfig.RingsRatios);
            ChangeAllRingsColors(powerStructureConfig.RingOnHoverColor);
            _clickHelper.OnEnterHover += ActivateRingSprites;
            _clickHelper.OnExitHover += DeactivateRingSprites;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanSelected += ActivateRingSpritesWithLock;
            Systems.MovementSystem.HerosMovementSystem.TempHeroMovementManager.OnAnyShamanDeselected += DeactivateRingSpritesWithLock;
        }
        private void FixedUpdate()
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                if (_ringIsColliding[i] == _ringHandlers[i].ColliderTargetingArea.IsColliding) continue;
                _ringIsColliding[i] = _ringHandlers[i].ColliderTargetingArea.IsColliding;

                ChangeAllRingsColors(_defaultColor);
                ChangeCurrentProximityRingColor(_activeColor);

            }
        }
        public void ToggleShowRings(PowerStructureConfig powerStructureConfig, bool state) //TEMP
        {
            if (state)
            {
                ScaleCircles(powerStructureConfig.Range,powerStructureConfig.RingsRatios);
                ActivateRingSprites();
                ChangeAllRingsColors(powerStructureConfig.RingOnHoverColor);
            }
            else
            {
                float[] defaultRingRatios = { 1, 1, 1 };
                ScaleCircles(1,defaultRingRatios);
                DeactivateRingSprites();
            }
            
        }
        private void ChangeAllRingsColors(Color color)
        {
            foreach (var ring in _ringHandlers)
            {
                ring.ChangeColor(color);
            }
        }
        private void ChangeCurrentProximityRingColor(Color color)
        {
            foreach (var ring in _ringHandlers)
            {
                if (!ring.ColliderTargetingArea.IsColliding) continue;
                ring.ChangeColor(color);
                break;
            }
        }
        private void ScaleCircles(float circleRange,float[] ringsRanges)
        {
            for (int i = 0; i < _ringHandlers.Length; i++)
            {
                _ringHandlers[i].Scale(circleRange * ringsRanges[i]);
            }
        }

        #region SpriteActivationToggle
        private void ActivateRingSprites()
        {
            ToggleActiveSprite(true,false);
            
        }
        private void ActivateRingSpritesWithLock()
        {
            _lockSpriteToggle = true;
            ToggleActiveSprite(true,true);
            
        }
        private void DeactivateRingSprites()
        {
            ToggleActiveSprite(false,false);
            
        }
        private void DeactivateRingSpritesWithLock()
        {
            _lockSpriteToggle = false;
            ToggleActiveSprite(false,true);
            
        }

        private void ToggleActiveSprite(bool state, bool lockOverride)
        {
            if (!lockOverride)
                if(_lockSpriteToggle) return;

            foreach (var ringHandler in _ringHandlers)
            {
                ringHandler.ToggleSprite(state);
            }
        }
        #endregion
    }
}