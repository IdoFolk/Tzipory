using System;
using System.Collections;
using Cinemachine;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Systems.CameraSystem
{
    public class CameraHandler : MonoBehaviour
    {
        private const float ORTHOGRAPHIC_DETECT_RANGE = 0.2f;
        [SerializeField] private CameraSettings cameraSettings;
        [Header("ON/OFF")] 
        [SerializeField] private bool _enableCameraMovement = false;
        [SerializeField] private bool _enableEdgeScroll = true;
        

        [Header("Gameobjects")] 
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private Transform _cameraFollowObject;
        [SerializeField] private float _cameraMoveToPosSpeed;

        public static CameraSettings CameraSettings { get; set; }

        public Camera MainCamera => _mainCamera;
        private Vector2 _edgeScrollBorder;
        private Vector2 _cameraStartPosition;
        private float _targetOrthographicSize;
        private readonly Vector3 _lockedCameraPosition = new Vector3(0,0,-80);
        private CinemachineTransposer _cinemachineTransposer;

        private void Awake()
        {
            if (Camera.allCameras.Length > 1)
                Destroy(gameObject);
            LockCamera(true);
        }

        private void Start()
        {
            _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _cinemachineTransposer.m_XDamping = cameraSettings.XDamping;
            _cinemachineTransposer.m_YDamping = cameraSettings.YDamping;
        }

        private void Update()
        {

            if (_enableCameraMovement) //option to disable camera movement and zoom
            {
                Vector3 inputDir = new Vector3(0, 0, 0); //resetting the input direction
                
                //WASD Detection
                if (Input.GetKey(KeyCode.W)) inputDir.y = +1f;
                if (Input.GetKey(KeyCode.S)) inputDir.y = -1f;
                if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
                if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

                //Edge Scrolling Detection
                if (_enableEdgeScroll)
                {
                    if (Input.mousePosition.x < cameraSettings.EdgeScrollDetectSize) inputDir.x = -1f;
                    if (Input.mousePosition.y < cameraSettings.EdgeScrollDetectSize) inputDir.y = -1f;
                    if (Input.mousePosition.x > Screen.width - cameraSettings.EdgeScrollDetectSize) inputDir.x = +1f;
                    if (Input.mousePosition.y > Screen.height - cameraSettings.EdgeScrollDetectSize) inputDir.y = +1f;
                }

                //Mouse Scroll Zoom 
                if (Input.mouseScrollDelta.y > 0)
                {
                    _targetOrthographicSize -= cameraSettings.ZoomAmount;
                    StartCoroutine(nameof(WaitForZoom));
                }

                if (Input.mouseScrollDelta.y < 0)
                {
                    _targetOrthographicSize += cameraSettings.ZoomAmount;
                    StartCoroutine(nameof(WaitForZoom));
                }


                //setting Variables
                var cameraTransform = transform;
                var cameraPosition = _cameraFollowObject.position;
                var orthographicSize = _mainCamera.orthographicSize;
                //setting the input direction to correspond with camera direction
                var moveDir = cameraTransform.up * inputDir.y + cameraTransform.right * inputDir.x;

                //moving the camera
                cameraPosition += moveDir * (cameraSettings.MoveSpeed * Time.deltaTime);
                var orthographicSizeX = orthographicSize * cameraSettings.EdgeDampingX;
                var orthographicSizeY = orthographicSize * cameraSettings.EdgeDampingY;
                //clamping the camera to the borders of the map
                cameraPosition.x = Mathf.Clamp(cameraPosition.x, -(_edgeScrollBorder.x - orthographicSizeX),
                    _edgeScrollBorder.x - orthographicSizeX);
                cameraPosition.y = Mathf.Clamp(cameraPosition.y, -(_edgeScrollBorder.y - orthographicSizeY),
                    _edgeScrollBorder.y - orthographicSizeY);
                _cameraFollowObject.position = cameraPosition;

                //moving + clamping the camera zoom

                _targetOrthographicSize =
                    Mathf.Clamp(_targetOrthographicSize, cameraSettings.ZoomMin, cameraSettings.ZoomMax);
                _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                    _cinemachineVirtualCamera.m_Lens.OrthographicSize, _targetOrthographicSize,
                    Time.deltaTime * cameraSettings.ZoomSpeed);
            }
        }

        public void SetCameraSettings(Vector2 cameraBorders,bool overWrite ,Vector2 startPos)
        {
            _edgeScrollBorder = cameraBorders;
            if (overWrite)
            {
                _cameraStartPosition = startPos;
            }
        }

        public void ResetCamera()
        {
            _cameraFollowObject.position = new Vector3(_cameraStartPosition.x,_cameraStartPosition.y,-80);
            _targetOrthographicSize = cameraSettings.ZoomStartValue;
        }

        public void LockCamera(bool state)
        {
            if (state)
            {
                _enableCameraMovement = false;
                _cameraFollowObject.position = _lockedCameraPosition;
                _cinemachineVirtualCamera.m_Lens.OrthographicSize = cameraSettings.ZoomStartValue;
            }
            else
            {
                _enableCameraMovement = true;
            }
        }

        public void SetCameraPosition(Vector2 eventPosition)
        {
            var newPos = new Vector3(eventPosition.x, eventPosition.y, -80);
            _cameraFollowObject.position = newPos;
        }
        IEnumerator WaitForZoom()
        {
            _cinemachineTransposer.m_XDamping = 0f;
            _cinemachineTransposer.m_YDamping = 0f;

            while (true)
            {
                if (_cinemachineVirtualCamera.m_Lens.OrthographicSize <= (_targetOrthographicSize + ORTHOGRAPHIC_DETECT_RANGE) && _cinemachineVirtualCamera.m_Lens.OrthographicSize >= (_targetOrthographicSize - ORTHOGRAPHIC_DETECT_RANGE))
                {
                    _cinemachineTransposer.m_XDamping = cameraSettings.XDamping;
                    _cinemachineTransposer.m_YDamping = cameraSettings.YDamping;
                    yield break;
                }
                yield return null;
            }
        }
    }
}