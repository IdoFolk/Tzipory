using System;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Shaman : UnitEntity, IPoolable<Shaman>
{

    public event Action<Shaman> OnDispose;

    [SerializeField] private EnemyTargeter enemyTargeter;

    public override void Init(UnitEntityConfig parameter)
    {
        base.Init(parameter);
        EnemyTargeter.Init(this);
        EntityCombatComponent.OnKill += GetEnergyFromKill;
    }

    private void GetEnergyFromKill(UnitEntity killedEntity)
    {
        
    }

    public EnemyTargeter EnemyTargeter { get => enemyTargeter; }
}
