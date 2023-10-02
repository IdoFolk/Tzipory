using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;
using Sirenix.OdinInspector;

public class TEMP_MapManager : MonoBehaviour
{
    [SerializeField] private TEMP_NodeObject[] _nodeObjects;
    [SerializeField] private bool _unLockAll;
    [Header("Camera Control")]
    [SerializeField] private Vector2 _cameraBorders;
    [SerializeField] private bool _overwriteCameraStartPosition;
    [SerializeField,ShowIf("_overwriteCameraStartPosition")] private Vector2 _cameraStartPosition;
    [SerializeField] private bool _enableGizmos;
    
    private bool[] _nodeLockState;
    private bool[] _nodeCompletedState;

    private void Awake()
    {
        //GameManager.CameraHandler.SetCameraSettings(_cameraBorders,_overwriteCameraStartPosition,_cameraStartPosition);
        //GameManager.CameraHandler.ResetCamera();
        
        _nodeLockState = GameManager.GameData.NodeLockStatState;
        _nodeCompletedState  = GameManager.GameData.NodeCompletedState;

        if (_unLockAll)
        {
            foreach (var nodeObject in _nodeObjects)
                nodeObject.Unlock();
            
            return;
        }

        for (int i = 0; i < _nodeObjects.Length; i++)
        {
            if (_nodeLockState[i])
            {
                _nodeObjects[i].Init(_nodeCompletedState[i],_nodeLockState[i]);
                // _nodeObjects[i].Unlock();
                // if (_nodeCompletedState[i])
                //     _nodeObjects[i].Completed();
            }
            else
            {
                _nodeObjects[i].Lock();
            }
        }
    }

    private void Start()
    {
        GameManager.CameraHandler.LockCamera(true);
    }

    private void OnDrawGizmos()
    {
        if (_enableGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, _cameraBorders * 2);
        }
    }
}