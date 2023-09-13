using System.Collections.Generic;
using Helpers;
using UnityEngine;
using PathCreation;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.ConfigFiles.WaveSystemConfig;
using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;
using Tzipory.WaveSystem;
using Random = UnityEngine.Random;


public class WaveSpawner : MonoBehaviour , IProgress
{
    [SerializeField, HideInInspector] private Color _color;
    [SerializeField,HideInInspector] private int _id;
    [SerializeField] private Transform _waveIndicatorPosition;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private PathCreator myPathCreator;

    private List<IProgress> _completedEnemyGroups;

    private List<EnemyGroupConfig> _enemyGroupsConfig;

    private int _currentEnemyGroupIndex;
    
    private float _delayBetweenEnemyGroup;

    private List<EnemyGroup> _activeEnemyGroup;

    public int ID => _id;

    public Color WaveSpawnerColor => _color;

    public bool IsSpawning { get; private set; }
    
    public float CompletionPercentage { get; }

    public int TotalNumberOfEnemiesPreWave { get; private set; }

    public Transform WaveIndicatorPosition => _waveIndicatorPosition;

    private bool IsDoneActiveEnemyGroup
    {
        get
        {
            if (_activeEnemyGroup == null)
                return false;

            foreach (var enemyGroup in _activeEnemyGroup)
            {
                if (!enemyGroup.IsDone)
                    return false;
            }

            return true;
        }
    }

    public bool IsDone
    {
        get
        {
            if (_completedEnemyGroups == null)
                return false;
            
            if (_completedEnemyGroups.Count != _enemyGroupsConfig.Count) 
                return false;
            
            foreach (var completedEnemyGroup in _completedEnemyGroups)
            {
                if (!completedEnemyGroup.IsDone)
                    return false;
            }
            
            return true;
        }
    }

    public bool IsActiveThisWave => _enemyGroupsConfig.Count != 0;

    private void Awake()
    {
        Level.AddWaveSpawner(this);
        _enemyGroupsConfig = new List<EnemyGroupConfig>();
    }

    public void Init(WaveSpawnerConfig waveSpawnerConfig)
    {
        _activeEnemyGroup = new List<EnemyGroup>();
        _completedEnemyGroups = new List<IProgress>();
        _enemyGroupsConfig = waveSpawnerConfig.EnemyGroups;
        _delayBetweenEnemyGroup = waveSpawnerConfig.DelayBetweenEnemyGroup;
        _currentEnemyGroupIndex = 0;
        
        foreach (var enemyGroupSerializeData in _enemyGroupsConfig)
            TotalNumberOfEnemiesPreWave += enemyGroupSerializeData.TotalSpawnAmount;

        IsSpawning = false;
    }

    public void StartSpawning() =>
        IsSpawning = true;

    private void Update()
    {
        if (!IsSpawning)
            return;
        
        if (IsDone || _activeEnemyGroup == null)
            return;
        
        if (IsDoneActiveEnemyGroup)
        {
            _completedEnemyGroups.AddRange(_activeEnemyGroup);
            _activeEnemyGroup.Clear();

            if (!TryGetNextEnemyGroup())
            {
                IsSpawning = false;
                return;
            }
            
            Debug.Log($"<color={ColorLogHelper.WAVE_MANAGER_COLOR}>WaveManager:</color> start enemyGroup-{_currentEnemyGroupIndex} at {gameObject.name}");
        }

        foreach (var enemyGroup in _activeEnemyGroup)
        {
            if (!enemyGroup.TryGetEnemy(out var entityConfig))
                continue;
            
            var enemy = PoolManager.EnemyPool.GetObject();
            
            enemy.Init(entityConfig);
            enemy.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            var enemyMoveComponent = enemy.GetComponent<MovementOnPath>();//temp!
            enemyMoveComponent.SetPath(myPathCreator);
            enemyMoveComponent.AdvanceOnPath();
        }
    }

    private bool TryGetNextEnemyGroup()
    {
        if (_enemyGroupsConfig.Count == 0) return false;
        
        if (_currentEnemyGroupIndex >= _enemyGroupsConfig.Count)
        {
            Debug.Log($"<color={ColorLogHelper.WAVE_MANAGER_COLOR}>WaveManager:</color> No more enemy group for {gameObject.name}");
            IsSpawning = false;
            return false;
        }
        
        _activeEnemyGroup.Add(new EnemyGroup(_enemyGroupsConfig[_currentEnemyGroupIndex]));//add to pool

        _currentEnemyGroupIndex++;

        if (_currentEnemyGroupIndex == _enemyGroupsConfig.Count) return true;
        
        while (_currentEnemyGroupIndex >= _enemyGroupsConfig.Count && _enemyGroupsConfig[_currentEnemyGroupIndex].StartType == ActionStartType.WithPrevious)
        {
            _activeEnemyGroup.Add(new EnemyGroup(_enemyGroupsConfig[_currentEnemyGroupIndex]));//add to pool
            
            _currentEnemyGroupIndex++;
        }

        return true;
    }
    
    public void SetColor(Color color)=>
        _color = color;

    public void SetId(int id) => _id = id;

    private void OnDrawGizmos()
    {
        Gizmos.color = WaveSpawnerColor;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.gray;                 
        Gizmos.DrawSphere(_waveIndicatorPosition.position, 0.5f);     
    }
}