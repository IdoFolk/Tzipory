using System;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Shaman : UnitEntity, IPoolable<Shaman>
{

    public event Action<Shaman> OnDispose;

    [SerializeField] private EnemyTargeter enemyTargeter;

    private void Start()
    {
        EnemyTargeter.Init(this);
    }

    public EnemyTargeter EnemyTargeter { get => enemyTargeter; }
}
