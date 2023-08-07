using UnityEngine;

public class TEMP_MapManager : MonoBehaviour
{
    [SerializeField] private TEMP_NodeObject[] _nodeObjects;

    private bool[] _nodeLockState;
    private bool[] _nodeCompletedState;

    private void Awake()
    {
        _nodeLockState = GameManager.GameData.NodeLockStatState;
        _nodeCompletedState  = GameManager.GameData.NodeCompletedState;
        
        for (int i = 0; i < _nodeObjects.Length; i++)
        {
            if (_nodeLockState[i])
            {
                _nodeObjects[i].Unlock();
                if (_nodeCompletedState[i])
                    _nodeObjects[i].Completed();
            }
            else
            {
                _nodeObjects[i].Lock();
            }
        }
    }
}