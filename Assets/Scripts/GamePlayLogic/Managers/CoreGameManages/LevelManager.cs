using System;
using GameplayeLogic.Managers;
using GamePlayLogic.Managers;
using Sirenix.OdinInspector;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Leval;
using Tzipory.SerializeData;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Systems.SceneSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static event Action<bool> OnEndGame;

    private PoolManager _poolManager;
   
    public static PartyManager PartyManager { get; private set; }
    public static EnemyManager EnemyManager { get; private set; }
    public static WaveManager WaveManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static CoreTemple CoreTemplete { get; private set; }
    
    public bool IsGameRunning { get; private set; }
    
    [SerializeField,TabGroup("Party manager")] private ShamanConfig[] _shamanConfigs;

    [SerializeField, TabGroup("Level manager"),Tooltip("Make the level that you can lose or win the game")]
    private bool _cantLose;
    [SerializeField, TabGroup("Level manager")]
    private Transform _levelParent;
    [SerializeField, TabGroup("Level manager")]
    private Transform _waveIndicatorParent;
    [Header("Testing")]
    [SerializeField, TabGroup("Level manager")]
    private LevelConfig _levelConfig;
    [SerializeField, TabGroup("Spawn parents")]
    private Transform _shamanParent;
    [SerializeField, TabGroup("Spawn parents")]
    private Transform _enemiesParent;
    
    private void Awake()
    {
        UIManager = new UIManager();
        _poolManager = new PoolManager();
        
        if (GameManager.GameData == null)//for Testing(Start form level scene)
        {
            var partyData = new PartySerializeData();
            partyData.Init(_shamanConfigs);
            PartyManager = new PartyManager(partyData,_shamanParent);
        }
        else
        {
            _levelConfig = GameManager.GameData.LevelConfig;
            PartyManager = new PartyManager(GameManager.PlayerManager.PlayerSerializeData.PartySerializeData,_shamanParent);
        }

        Instantiate(_levelConfig.Level, _levelParent);
        EnemyManager = new EnemyManager(_enemiesParent);
        WaveManager  = new WaveManager(_levelConfig,_waveIndicatorParent);//temp!
        CoreTemplete = FindObjectOfType<CoreTemple>();//temp!!!
    }

    private void Start()
    {
        PartyManager.SpawnShaman();
        WaveManager.StartLevel();
        UIManager.Initialize();
        GAME_TIME.SetTimeStep(1);
        IsGameRunning = true;
    }

    private void Update()
    {
        if (!IsGameRunning)
            return;
        
        WaveManager.UpdateLevel();

        if (_cantLose)
            return;

        if (CoreTemplete.IsEntityDead)
            EndGame(false);

        if (WaveManager.AllWaveAreDone && EnemyManager.AllEnemiesArDead)
            EndGame(true);
    }
    
    private void OnDestroy()
    {
        UIManager.Dispose();
        EnemyManager.Dispose();
        PartyManager.Dispose();
        WaveManager.Dispose();
        
        PartyManager = null;
        EnemyManager = null;
        WaveManager = null;
    }

    private void EndGame(bool isWon)
    {
        if (!IsGameRunning) return;
        
        GAME_TIME.SetTimeStep(0);

        if (isWon)
            GameManager.GameData?.SetCompletedNodeStat(_levelConfig.LevelId,true);

        OnEndGame?.Invoke(isWon);
        IsGameRunning = false;
    }
    
    public void Continue()
    {
        GAME_TIME.SetTimeStep(1);

        if (GameManager.SceneHandler != null)//for testing so you can rest the level if you start from the coreGame scene
            GameManager.SceneHandler.LoadScene(SceneType.Map);
        else
            SceneManager.LoadScene(3);//reset the CoreGame scene so you can play again
    }

#if UNITY_EDITOR
    [Button("Win")]
    public void Win()
    {
        EndGame(true);
    }

    [Button("Lose")] 
    public void Lose()
    {
        EndGame(false);
    }

#endif
}