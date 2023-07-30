using System;
using GameplayeLogic.Managersp;
using GamePlayLogic.Managers;
using Systems.DataManagerSystem;
using Tzipory.ConfigFiles;
using Tzipory.Systems.SceneSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static ISceneHandler SceneHandler { get; private set; }
    
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private SceneHandler _sceneHandler;
    
    private static PlayerManager _playerManager;
    //MapData
    //GameData

    public static PlayerManager PlayerManager => _playerManager;

    private void Awake()
    {
        if (SceneHandler == null)
            SceneHandler = _sceneHandler;
    }

    void Start()
    {
        _sceneHandler.LoadScene(SceneType.MainMenu);
        
        var playerSerializeData = DataManager.DataRequester.GetData<PlayerSerializeData>(_playerConfig); 
        _playerManager = new PlayerManager(playerSerializeData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Test
    [ContextMenu("LoadMap")]
    public void LoadScene()
    {
        _sceneHandler.LoadScene(SceneType.Map);
    }

    #endregion

    private void OnValidate()
    {
        if (_sceneHandler == null)
            _sceneHandler = FindObjectOfType<SceneHandler>();
    }
}
