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

    public static GameData GameData { get; private set; }
    public static PlayerManager PlayerManager { get; private set; }

    private void Awake()
    { 
        if (SceneHandler == null)
            SceneHandler = _sceneHandler;

        GameData = new GameData();
    }

    void Start()
    {
        SceneHandler.LoadScene(SceneType.MainMenu);
        
        var playerSerializeData = DataManager.DataRequester.GetData<PlayerSerializeData>(_playerConfig); 
        PlayerManager = new PlayerManager(playerSerializeData);
    }

    #region Test
    [ContextMenu("LoadMap")]
    public void LoadScene()
    {
        SceneHandler.LoadScene(SceneType.Map);
    }

    private void OnDestroy()
    {
        GameData = null;
        PlayerManager = null;
    }

    #endregion

    private void OnValidate()
    {
        if (_sceneHandler == null)
            _sceneHandler = FindObjectOfType<SceneHandler>();
    }
}
