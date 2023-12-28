using System;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Enemy : UnitEntity , IPoolable<Enemy>
{
    public EnemyType EnemyType { get; private set; }
    public override void Init(UnitEntityConfig parameter)
    {
        base.Init(parameter);
        EnemyType = parameter.EnemyType;
        
    }

    public event Action<Enemy> OnDispose;
}

public enum EnemyType
{
    Madu,
    Gallus
}