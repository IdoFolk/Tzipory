using System;
using System.Collections;
using Cinemachine;
using Tzipory.Helpers;
using UnityEngine;

namespace Tzipory.Systems.CameraSystem
{
    public class CameraHandler : MonoBehaviour
    {
        private const float ORTHOGRAPHIC_DETECT_RANGE = 0.2f;
        private const float CAMERA_MOVEMENT_DETECT_RANGE = 0.5f;
        private const float MAX_ZOOM_DEFINED_BY_BORDERS = 9f;

        private const float FULL_HD_PIXELS_X = 1920;
        private const float FULL_HD_PIXELS_y = 1080;

        [SerializeField, Tooltip("attach a camera setting config file to determine all of the camera variables")]
        private CameraSettings _cameraSettings;

        [Header("ON/OFF")] [SerializeField, Tooltip("toggle camera movement and zoom")]
        private bool _enableCameraMovement = false;

        [SerializeField, Tooltip("toggle mouse edge scroll camera movement")]
        private bool _enableEdgeScroll = true;


        [Header("Gameobjects")] [SerializeField]
        private Camera _mainCamera;

        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private Transform _cameraFollowObject;

        public static CameraSettings CameraSettings { get;}
        public Camera MainCamera => _mainCamera;
        public readonly Vector3 LockedCameraPosition = new Vector3(0, -3, -80);
        public readonly int LockedCameraZoom = 9;

        private Vector2 _edgeScrollBorder;
        private Vector2 _cameraStartPosition;
        private float _cameraStartZoom;
        private float _targetOrthographicSize;
        private CinemachineTransposer _cinemachineTransposer;

        private float _currentAspectRatioX;
        private float _currentAspectRatioY;
        private float _edgePaddingX;
        private float _edgePaddingY;
        private float _zoomPadding;

        private void Awake()
        {
            //only 1 camera in the scene
            if (Camera.allCameras.Length > 1)
                Destroy(gameObject);
        }

        private void Start()
        {
            //check if a camera settings config file is attached
            if (_cameraSettings is null)
            {
                string cameraSettingNullLog =
                    ColorLogHelper.SetColorToString("Camera Settings", ColorLogHelper.CAMERA_HANDLER);
                throw new Exception($"{cameraSettingNullLog} is null"); //stop program?
            }

            //caching CinemachineTransposer
            _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _cinemachineTransposer.m_XDamping = _cameraSettings.XDamping;
            _cinemachineTransposer.m_YDamping = _cameraSettings.YDamping;
            _edgePaddingX = _cameraSettings.DefaultEdgePaddingX;
            _edgePaddingY = _cameraSettings.DefaultEdgePaddingY;
            LockCamera(LockedCameraPosition, LockedCameraZoom);
        }

        private void Update()
        {
            //here we control the camera movement and zoom

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
                    if (Input.mousePosition.x < (Screen.width * _cameraSettings.EdgeScrollDetectSizeX))
                        inputDir.x = -1f;
                    if (Input.mousePosition.y < (Screen.height * _cameraSettings.EdgeScrollDetectSizeY))
                        inputDir.y = -1f;
                    if (Input.mousePosition.x > Screen.width - (Screen.width * _cameraSettings.EdgeScrollDetectSizeX))
                        inputDir.x = +1f;
                    if (Input.mousePosition.y > Screen.height - (Screen.height * _cameraSettings.EdgeScrollDetectSizeY))
                        inputDir.y = +1f;
                }

                //Mouse Scroll Zoom 
                if (Input.mouseScrollDelta.y > 0)
                {
                    _targetOrthographicSize -= _cameraSettings.ZoomChangeValue;
                    StartCoroutine(ChangeDampingForZoom(0, 0));
                }

                if (Input.mouseScrollDelta.y < 0)
                {
                    _targetOrthographicSize += _cameraSettings.ZoomChangeValue;
                    StartCoroutine(ChangeDampingForZoom(0, 0));
                }


                //setting Variables
                var cameraTransform = transform;
                var cameraPosition = _cameraFollowObject.position;
                var orthographicSize = _mainCamera.orthographicSize;

                //setting the input direction to correspond with camera direction
                var moveDir = cameraTransform.up * inputDir.y + cameraTransform.right * inputDir.x;

                //determine the camera speed according to the camera zoom
                float currentZoomNormalizedValue =
                    (_cinemachineVirtualCamera.m_Lens.OrthographicSize - _cameraSettings.ZoomMinClamp) /
                    (_zoomPadding - _cameraSettings.ZoomMinClamp);
                float zoomSpeedChangeValue = currentZoomNormalizedValue * _cameraSettings.CameraSpeedZoomChangeValue;
                float fixedCameraSpeed = _cameraSettings.MoveSpeedMinimum + zoomSpeedChangeValue;

                //moving the camera
                cameraPosition += moveDir * (fixedCameraSpeed * Time.deltaTime);
                var orthographicSizeX = orthographicSize * (_edgePaddingX);
                var orthographicSizeY = orthographicSize * (_edgePaddingY);

                //clamping the camera to the borders of the map
                cameraPosition.x = Mathf.Clamp(cameraPosition.x, -(_edgeScrollBorder.x - orthographicSizeX),
                    _edgeScrollBorder.x - orthographicSizeX);
                cameraPosition.y = Mathf.Clamp(cameraPosition.y, -(_edgeScrollBorder.y - orthographicSizeY),
                    _edgeScrollBorder.y - orthographicSizeY);
                _cameraFollowObject.position = cameraPosition;


                //moving + clamping the camera zoom
                _targetOrthographicSize =
                    Mathf.Clamp(_targetOrthographicSize, _cameraSettings.ZoomMinClamp, _zoomPadding);
                _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                    _cinemachineVirtualCamera.m_Lens.OrthographicSize, _targetOrthographicSize,
                    Time.deltaTime * _cameraSettings.ZoomSpeed);
            }
        }

        public void SetCameraSettings(Vector2 cameraBorders, bool overWrite, Vector2 startPos, float startZoom)
        {
            _edgeScrollBorder = cameraBorders;
            if (overWrite)
            {
                _cameraStartPosition = startPos;
                _cameraStartZoom = startZoom;
            }
        }

        public void ResetCamera()
        {
            //calculating the current aspect ratio according to screen resolution
            _currentAspectRatioX = FULL_HD_PIXELS_X / _mainCamera.pixelWidth;
            _currentAspectRatioY = FULL_HD_PIXELS_y / _mainCamera.pixelHeight;

            //calculating the current padding for movement borders and zoom
            _edgePaddingX = _cameraSettings.DefaultEdgePaddingX / _currentAspectRatioX;
            _edgePaddingY = _cameraSettings.DefaultEdgePaddingY / _currentAspectRatioY;
            _zoomPadding = MAX_ZOOM_DEFINED_BY_BORDERS * _currentAspectRatioX;
            if (_zoomPadding > _cameraSettings.ZoomMaxClamp) _zoomPadding = _cameraSettings.ZoomMaxClamp;

            //resetting the camera position and zoom
            LockCamera();
            if (_cameraStartZoom > _cameraSettings.ZoomMinClamp && _cameraStartZoom < _zoomPadding)
            {
                _targetOrthographicSize = _cameraStartZoom;
                _mainCamera.orthographicSize = _cameraStartZoom;
            }
            else
            {
                _targetOrthographicSize = _cameraSettings.ZoomDefaultStartValue;
                _mainCamera.orthographicSize = _cameraSettings.ZoomDefaultStartValue;
            }
            _cameraFollowObject.position = new Vector3(_cameraStartPosition.x, _cameraStartPosition.y, -80);
            _mainCamera.transform.position = new Vector3(_cameraStartPosition.x, _cameraStartPosition.y, -80);
            UnlockCamera();
        }

        public void UnlockCamera()
        {
            _enableCameraMovement = true;
            _mainCamera.GetComponent<CinemachineBrain>().enabled = true;
        }
        public void LockCamera()
        {
            _mainCamera.GetComponent<CinemachineBrain>().enabled = false;
            _enableCameraMovement = false;
        }
        public void LockCamera(Vector2 lockedCameraPos, int lockedCameraZoom)
        {
            _mainCamera.GetComponent<CinemachineBrain>().enabled = false;
            _mainCamera.transform.position = new Vector3(lockedCameraPos.x, lockedCameraPos.y, -80);
            _mainCamera.orthographicSize = lockedCameraZoom;
            _enableCameraMovement = false;
        }
        
        public void SetCameraPosition(Vector2 eventPosition)
        {
            //move the camera to Event Position
            var newPos = new Vector3(eventPosition.x, eventPosition.y, -80);
            StartCoroutine(ChangeDampingForEventTransition(_cameraSettings.EventTransitionDampingX,
                _cameraSettings.EventTransitionDampingY));
            _cameraFollowObject.position = newPos;
        }

        IEnumerator ChangeDampingForZoom(float xdamping, float ydamping)
        {
            //change the damping speed to zero when zooming in and out
            _cinemachineTransposer.m_XDamping = xdamping;
            _cinemachineTransposer.m_YDamping = ydamping;

            while (true)
            {
                if (_cinemachineVirtualCamera.m_Lens.OrthographicSize <=
                    (_targetOrthographicSize + ORTHOGRAPHIC_DETECT_RANGE) &&
                    _cinemachineVirtualCamera.m_Lens.OrthographicSize >=
                    (_targetOrthographicSize - ORTHOGRAPHIC_DETECT_RANGE))
                {
                    _cinemachineTransposer.m_XDamping = _cameraSettings.XDamping;
                    _cinemachineTransposer.m_YDamping = _cameraSettings.YDamping;
                    yield break;
                }

                yield return null;
            }
        }

        IEnumerator ChangeDampingForEventTransition(float xdamping, float ydamping)
        {
            //change the damping speed when moving to an Event Position
            _cinemachineTransposer.m_XDamping = xdamping;
            _cinemachineTransposer.m_YDamping = ydamping;

            while (true)
            {
                if (_mainCamera.transform.position.x <=
                    (_cameraFollowObject.position.x + CAMERA_MOVEMENT_DETECT_RANGE) &&
                    _mainCamera.transform.position.x >= (_cameraFollowObject.position.x - CAMERA_MOVEMENT_DETECT_RANGE))
                {
                    if (_mainCamera.transform.position.y <=
                        (_cameraFollowObject.position.y + CAMERA_MOVEMENT_DETECT_RANGE) &&
                        _mainCamera.transform.position.y >=
                        (_cameraFollowObject.position.y - CAMERA_MOVEMENT_DETECT_RANGE))
                    {
                        _cinemachineTransposer.m_XDamping = _cameraSettings.XDamping;
                        _cinemachineTransposer.m_YDamping = _cameraSettings.YDamping;
                        yield break;
                    }
                }

                yield return null;
            }
        }
    }
}