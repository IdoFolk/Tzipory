using System;
using System.Collections;
using Tzipory.GameplayLogic.UIElements;
using Tzipory.Helpers;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tzipory.Systems.MovementSystem.HerosMovementSystem
{
    public class TempHeroMovementManager : MonoSingleton<TempHeroMovementManager>
    {
        public static event Action OnAnyShamanSelected;
        public static event Action OnAnyShamanDeselected;

        public event Action<Vector3> OnMove;

        private Temp_HeroMovement _currentTarget;
        private Temp_HeroMovement _waitForTotemTarget;
        private Camera _camera;

        private bool _isValidClick;

        private float _previousTimeRate;

        [SerializeField] private AnimationCurve _startSlowTimeCurve;
        [SerializeField] private AnimationCurve _endSlowTimeCurve;
        [SerializeField] private float _slowTimeTransitionTime;
        [SerializeField] private float _slowTime;
        
        [SerializeField] private Shadow _shadow;

        
        //temp?
        bool isCooldown;
        private void Start()
        {
            _shadow.gameObject.SetActive(false);
            isCooldown = false;
            _camera = Camera.main;
        }

        public void SelectTarget(Temp_HeroMovement  target, Sprite shadowSprite, float range)
        {
            if (isCooldown)
                return;
            _currentTarget = target;
            if (TotemPanelUIManager.TotemSelected)
            {
                _waitForTotemTarget = _currentTarget;
                _waitForTotemTarget.TotemPlaced += ClearTarget;
            }
            _shadow.SetShadow(target.transform, shadowSprite, range);
            Cursor.visible = false;
            _previousTimeRate = GAME_TIME.GetCurrentTimeRate;
            GAME_TIME.SetTimeStep(_slowTime,_slowTimeTransitionTime,_startSlowTimeCurve);
            OnAnyShamanSelected?.Invoke();
        }

        public void ClearTarget()
        {
            if (TotemPanelUIManager.TotemSelected) 
                _waitForTotemTarget.TotemPlaced -= ClearTarget;
            _currentTarget = null;
            _shadow.ClearShadow(false);
            Cursor.visible = true;
            isCooldown = true;
            StartCoroutine(SetIsCooldownWaitOneFrame(false));
            TotemPanelUIManager.ToggleTotemSelected(false);
            GAME_TIME.SetTimeStep(_previousTimeRate,_slowTimeTransitionTime,_endSlowTimeCurve);
            OnAnyShamanDeselected?.Invoke();
        }

        private IEnumerator SetIsCooldownWaitOneFrame(bool isIt)
        {
            yield return new WaitForSeconds(.1f);
            isCooldown = isIt;
        }

        private void PlaceShadowTotem()
        {
            _currentTarget = null;
            _shadow.ClearShadow(true);
            Cursor.visible = true;
            isCooldown = true;
            StartCoroutine(SetIsCooldownWaitOneFrame(false));
            TotemPanelUIManager.ToggleTotemSelected(false);
            GAME_TIME.SetTimeStep(_previousTimeRate,_slowTimeTransitionTime,_endSlowTimeCurve);
            OnAnyShamanDeselected?.Invoke();
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
                _currentTarget.SetTarget(worldPos);
                OnMove?.Invoke(worldPos);

                if (TotemPanelUIManager.TotemSelected) PlaceShadowTotem();
                else ClearTarget();
            }

            if (_currentTarget != null)
            {
                _isValidClick = true;
            }
        }
    }
}