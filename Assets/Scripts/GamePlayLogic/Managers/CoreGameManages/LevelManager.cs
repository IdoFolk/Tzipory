using System;
using GameplayeLogic.Managers;
using GamePlayLogic.Managers;
using Sirenix.OdinInspector;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Leval;
using Tzipory.SerializeData;
using Tzipory.SerializeData.LevalSerializeData;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static event Action<bool> OnEndGame;

    [SerializeField,TabGroup("Party manager")] private PartySerializeData _partySerializeData;
    
    private PoolManager _poolManager;
   
    public static PartyManager PartyManager { get; private set; }
    public static EnemyManager EnemyManager { get; private set; }
    public static WaveManager WaveManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static CoreTemple CoreTemplete { get; private set; }
    
    public bool IsGameRunning { get; private set; }
    
    [SerializeField, TabGroup("Level manager")]
    private Transform _levelParent;
    [SerializeField, TabGroup("Level manager")]
    private LevelConfig _levelConfig;
    
    private void Awake()
    {
        UIManager = new UIManager();
        _poolManager = new PoolManager();
        EnemyManager = new EnemyManager();
        PartyManager = new PartyManager(_partySerializeData,_levelConfig.EntityParent);
        WaveManager  = new WaveManager(_levelConfig,_levelParent);//temp!
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
        OnEndGame?.Invoke(isWon);
        IsGameRunning = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reset()
    {
        GAME_TIME.SetTimeStep(1);
        SceneManager.LoadScene(0);
    }
}