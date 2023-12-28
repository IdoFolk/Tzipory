using System;
using System.Collections.Generic;
using Tzipory;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Shaman : UnitEntity, IPoolable<Shaman>
{
    public Dictionary<int, SimpleStat> SimpleStats;
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
        Constant.StatsId energyStatType = Constant.StatsId.Energy;
        float energyGained = killedEntity.EntityStatComponent.GetStat(Constant.StatsId.Energy).CurrentValue;
        SimpleStats[(int)energyStatType].ChangeValue(energyGained,StatModifierType.Addition);
    }

    public EnemyTargeter EnemyTargeter { get => enemyTargeter; }
}
