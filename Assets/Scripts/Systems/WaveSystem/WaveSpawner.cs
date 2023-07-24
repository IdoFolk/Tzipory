using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;
using Tzipory.WaveSystem;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour , IProgress
{
    [SerializeField, HideInInspector] private Color _color;
    [SerializeField,HideInInspector] private int _id;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private PathCreator myPathCreator;

    private List<IProgress> _completedEnemyGroups;

    private EnemyGroupConfig[] _enemyGroups;

    private int _currentEnemyGroupIndex;
    
    private float _delayBetweenEnemyGroup;

    private List<EnemyGroup> _activeEnemyGroup;

    public int ID => _id;

    public Color WaveSpawnerColor => _color;

    public bool IsSpawning { get; private set; }
    
    public float CompletionPercentage { get; }

    public int TotalNumberOfEnemiesPreWave { get; private set; }

    private bool IsDoneActiveEnemyGroup
    {
        get
        {
            if (_activeEnemyGroup == null || _activeEnemyGroup.Count == 0)
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
            
            if (_completedEnemyGroups.Count != _enemyGroups.Length) 
                return false;
            
            foreach (var completedEnemyGroup in _completedEnemyGroups)
            {
                if (!completedEnemyGroup.IsDone)
                    return false;
            }
            return true;
        }
    }

    private void Awake()
    {
        Level.AddWaveSpawner(this);
    }

    public void Init(WaveSpawnerConfig waveSpawnerConfig)
    {
        _activeEnemyGroup = new List<EnemyGroup>();
        _completedEnemyGroups = new List<IProgress>();
        _enemyGroups = waveSpawnerConfig.EnemyGroups;
        _delayBetweenEnemyGroup = waveSpawnerConfig.DelayBetweenEnemyGroup;
        _currentEnemyGroupIndex = 0;
        
        foreach (var enemyGroupSerializeData in _enemyGroups)
            TotalNumberOfEnemiesPreWave += enemyGroupSerializeData.TotalSpawnAmount;

        if (!TryGetNextEnemyGroup())
            Debug.LogWarning("Not enemy group for the spawner");
    }

    private void Update()
    {
        if (IsDone || _activeEnemyGroup == null || _activeEnemyGroup.Count == 0)
            return;
        
        if (IsDoneActiveEnemyGroup)
        {
            _completedEnemyGroups.AddRange(_activeEnemyGroup);
            _activeEnemyGroup.Clear();
            
            if (!TryGetNextEnemyGroup())
                return;
            Debug.Log($"#<color=2eff00>WaveManager:</color> start enemyGroup-{_currentEnemyGroupIndex + 1}");
        }

        foreach (var enemyGroup in _activeEnemyGroup)
        {
            if (!enemyGroup.TryGetEnemy(out var entityConfig))
                return;
            
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
        if (_enemyGroups.Length == 0) return false;
        if (_currentEnemyGroupIndex >= _enemyGroups.Length) return false;
            
        _activeEnemyGroup.Add(new EnemyGroup(_enemyGroups[_currentEnemyGroupIndex]));
        _currentEnemyGroupIndex++;

        if (_currentEnemyGroupIndex == _enemyGroups.Length)
            return false;

        if (_enemyGroups[_currentEnemyGroupIndex].StartType == ActionStartType.WithPrevious)
            TryGetNextEnemyGroup();

        return true;
    }
    
    public void SetColor(Color color)=>
        _color = color;

    public void SetId(int id) => _id = id;

    private void OnDrawGizmos()
    {
        Gizmos.color = WaveSpawnerColor;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}