using System;
using System.Collections;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GameplayLogic.UI.CoreGameUI.HeroSelectionUI;
using Tzipory.GameplayLogic.VisualSystem.EffectType;
using Tzipory.Helpers;
using Tzipory.Systems.EntityComponents;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tzipory.Systems.MovementSystem.HerosMovementSystem
{
    public class TempHeroMovementManager : MonoSingleton<TempHeroMovementManager>
    {
        public static event Action OnAnyShamanSelected;
        public static event Action OnAnyShamanDeselected;
        
        private AgentMoveComponent _currentTarget;
        private Camera _camera;

        private bool _isValidClick;

        private float _previousTimeRate;

        [SerializeField] private AnimationCurve _startSlowTimeCurve;
        [SerializeField] private AnimationCurve _endSlowTimeCurve;
        [SerializeField] private float _slowTimeTransitionTime;
        [SerializeField] private float _slowTime;
        
        [SerializeField] private Shadow _shadow;
        
        private bool _isCooldown;
        
        private void Start()
        {
            _shadow.gameObject.SetActive(false);
            _isCooldown = false;
            _camera = GameManager.CameraHandler.MainCamera;
        }
        
        public void SelectTarget(UnitEntity target, Sprite shadowSprite, float range)
        {
            if (_isCooldown)
                return;
            _currentTarget = target.EntityMovementComponent.AgentMoveComponent;
            _shadow.SetShadow(target, shadowSprite, range);

            Cursor.visible = false;
            
            SlowMotionManager.Instance.StartSlowMotionEffects();
            HeroSelectionUI.Instance.Init(target);
            
            OnAnyShamanSelected?.Invoke();
        }

        private void ClearTarget()
        {
            _currentTarget = null;
            _shadow.ClearShadow();
            Cursor.visible = true;
            _isCooldown = true;
            StartCoroutine(SetIsCooldownWaitOneFrame(false));
            
            SlowMotionManager.Instance.EndSlowMotionEffects();
            HeroSelectionUI.Instance.HideSelectionUI();
            
            OnAnyShamanDeselected?.Invoke();
        }
        
        private IEnumerator SetIsCooldownWaitOneFrame(bool isIt)
        {
            yield return new WaitForSeconds(.1f);
            _isCooldown = isIt;
        }

        private void Update()
        {
            if (_currentTarget == null)
            {
                _isValidClick = false;
                return;
            }

            if (_shadow.IsOn)
            {
                if(Mouse.current.rightButton.wasPressedThisFrame)
                {
                    _shadow.ClearShadow();
                    ClearTarget();
                    return;
                }
                Vector3 newPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                newPos.z = 0f; //TEMP, needs to be set to same Z as shaman
                _shadow.transform.position = newPos;
            }
            
            if (Mouse.current.leftButton.wasPressedThisFrame && _isValidClick)
            {
                var screenPos = Mouse.current.position.ReadValue();
                var worldPos = _camera.ScreenToWorldPoint(screenPos);
                worldPos = new Vector3(worldPos.x, worldPos.y, 0);
                _currentTarget.SetAgentDestination(worldPos);

                ClearTarget();
            }

            if (_currentTarget != null)
            {
                _isValidClick = true;
            }
        }
    }
}