using System;
using TMPro;
using Tzipory.BaseScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem.HerosMovementSystem
{
    public class TempHeroMovementManager : MonoSingleton<TempHeroMovementManager>
    {
        public static event Action OnAnyShamanSelected;
        public static event Action OnAnyShamanDeselected;

        public event Action<Vector3> OnMove;

        private Temp_HeroMovement _currentTarget;
        private Camera _camera;

        private bool _isValidClick;

        [SerializeField] private Shadow _shadow;
       


        private void Start()
        {
            _shadow.gameObject.SetActive(false);
            _camera = Camera.main;
        }

        public void SelectTarget(Temp_HeroMovement  target)
        {
            _currentTarget = target;
            OnAnyShamanSelected?.Invoke();
        }
        public void SelectTarget(Temp_HeroMovement  target, Sprite shadowSprite, float range)
        {
            _currentTarget = target;
            _shadow.SetShadow(target.transform, shadowSprite, range);

            Cursor.visible = false;

            OnAnyShamanSelected?.Invoke();
        }

        public void ClearTarget()
        {
            _currentTarget = null;
            _shadow.ClearShadow();
            Cursor.visible = true;

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
                _currentTarget.SetTarget(worldPos);
                Debug.Log("Set new pos");
                OnMove?.Invoke(worldPos);

                ClearTarget();
            }

            if (_currentTarget != null)
            {
                _isValidClick = true;
            }
        }
    }
}