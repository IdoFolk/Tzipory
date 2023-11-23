using System;
using System.Collections;
using Tzipory.GameplayLogic.EntitySystem.Totems;
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

        public void SelectTarget(Temp_HeroMovement target, Sprite shadowSprite, float range)
        {
            if (isCooldown)
                return;
            _currentTarget = target;
            if (TotemPanelUIManager.TotemSelected.TryGetValue(target.Shaman.EntityInstanceID, out var value))
            {
                if (value)
                {
                    _currentTarget.TotemPlaced += TotemPlaced;
                }
                else
                {
                    TotemManager.Instance.TotemPanelUIManager.TotemPlacementUI.HideShadowTotem(_currentTarget.Shaman.EntityInstanceID);
                }
                
            }

            _shadow.SetShadow(target.transform, shadowSprite, range);
            Cursor.visible = false;
            if (GAME_TIME.GetCurrentTimeRate == _slowTime) return;
            _previousTimeRate = GAME_TIME.GetCurrentTimeRate;
            GAME_TIME.SetTimeStep(_slowTime, _slowTimeTransitionTime, _startSlowTimeCurve);
            OnAnyShamanSelected?.Invoke();
        }

        private void ClearTarget(int id = -1)
        {
            if (TotemPanelUIManager.TotemSelected is not null)
                TotemPanelUIManager.ToggleAllTotemsSelected(false);

            _currentTarget = null;
            _shadow.ClearShadow();
            Cursor.visible = true;
            isCooldown = true;
            StartCoroutine(SetIsCooldownWaitOneFrame(false));
            GAME_TIME.SetTimeStep(_previousTimeRate, _slowTimeTransitionTime, _endSlowTimeCurve);
            OnAnyShamanDeselected?.Invoke();
        }

        private IEnumerator SetIsCooldownWaitOneFrame(bool isIt)
        {
            yield return new WaitForSeconds(.1f);
            isCooldown = isIt;
        }

        private void PlaceShadowTotem(int id, Vector3 pos)
        {
            TotemManager.Instance.TotemPanelUIManager.TotemPlacementUI.PlaceShadowTotem(id, pos);
            ClearTarget(id);
        }

        private void TotemPlaced(Temp_HeroMovement target)
        {
            target.TotemPlaced -= TotemPlaced;
            TotemManager.Instance.TotemPanelUIManager.TotemPlacementUI.HideShadowTotem(target.Shaman.EntityInstanceID);
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
                if (Mouse.current.rightButton.wasPressedThisFrame)
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

                if (TotemPanelUIManager.TotemSelected.TryGetValue(_currentTarget.Shaman.EntityInstanceID, out var value))
                {
                    if (value)
                        PlaceShadowTotem(_currentTarget.Shaman.EntityInstanceID, worldPos);
                    else 
                        ClearTarget();
                }
                else
                    ClearTarget();
            }

            if (_currentTarget != null)
            {
                _isValidClick = true;
            }
        }
    }
}