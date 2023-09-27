using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraHandler : MonoBehaviour
{
    [Header("ON/OFF")] [SerializeField] private bool _enableCameraMovement = true;

    [Header("Gameobjects")] [SerializeField]
    private Camera _mainCamera;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Transform _cameraFollowObject;

    [Header("Parameters")] [SerializeField]
    private float _moveSpeed = 30f;

    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _zoomAmount = 1f;
    [SerializeField] private float _zoomMin = 2f;
    [SerializeField] private float _zoomMax = 10f;
    [SerializeField] private float _targetFieldOfView = 3f;
    [SerializeField] private int _edgeScrollDetectSize = 20;
    [SerializeField] private bool _enableEdgeScroll;
    [SerializeField] private float _xDamping;
    [SerializeField] private float _yDamping;
    [SerializeField] private float _edgeDampingX;
    [SerializeField] private float _edgeDampingY;
    [SerializeField] private Vector2 _edgeScrollBorder;

    private void Awake()
    {
        if (Camera.allCameras.Length > 1)
            Destroy(gameObject);

        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = _xDamping;
        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = _yDamping;
    }

    private void Start()
    {

    }

    private void Update()
    {
        HandleCameraMovement();
        HandleCameraZoom_FOV();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (_enableCameraMovement)
        {
            if (Input.GetKey(KeyCode.W)) inputDir.y = +1f;
            if (Input.GetKey(KeyCode.S)) inputDir.y = -1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;
            if (_enableEdgeScroll)
            {
                if (Input.mousePosition.x < _edgeScrollDetectSize) inputDir.x = -1f;
                if (Input.mousePosition.y < _edgeScrollDetectSize) inputDir.y = -1f;
                if (Input.mousePosition.x > Screen.width - _edgeScrollDetectSize) inputDir.x = +1f;
                if (Input.mousePosition.y > Screen.height - _edgeScrollDetectSize) inputDir.y = +1f;
            }
        }

        var transform1 = transform;
        var cameraPosition = _cameraFollowObject.position;
        Vector3 moveDir = transform1.up * inputDir.y + transform1.right * inputDir.x;

        cameraPosition += moveDir * (_moveSpeed * Time.deltaTime);
        var orthographicSizeX = _mainCamera.orthographicSize * _edgeDampingX;
        var orthographicSizeY = _mainCamera.orthographicSize * _edgeDampingY;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -(_edgeScrollBorder.x - orthographicSizeX), _edgeScrollBorder.x - orthographicSizeX);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -(_edgeScrollBorder.y - orthographicSizeY), _edgeScrollBorder.y - orthographicSizeY);
        _cameraFollowObject.position = cameraPosition;
    }

    private void HandleCameraZoom_FOV()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFieldOfView -= _zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFieldOfView += _zoomAmount;
        }

        _targetFieldOfView = Mathf.Clamp(_targetFieldOfView, _zoomMin, _zoomMax);
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            _cinemachineVirtualCamera.m_Lens.OrthographicSize, _targetFieldOfView, Time.deltaTime * _zoomSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero,_edgeScrollBorder * 2);
    }
}