using System;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using Sirenix.OdinInspector;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;

public class CoreTemple : BaseGameEntity, IEntityTargetAbleComponent
{
    [SerializeField] private PathCreator _patrolPath;
    
    [SerializeField]
    float _hp;

    [SerializeField,ReadOnly] private Stat _hpStat;

    public event Action<IEntityTargetAbleComponent> OnTargetDisable;
    public bool IsTargetAble => true;

    public PathCreator PatrolPath => _patrolPath;

    public EntityType EntityType => EntityType.Core;

    public Stat InvincibleTime => throw new System.NotImplementedException();

    public bool IsDamageable => true; //temp

    public Stat Health => _hpStat;
    public bool IsEntityDead => Health.CurrentValue <= 0;

    public StatusHandler StatusHandler => throw new System.NotImplementedException();

    public System.Action OnHealthChanged;

    //SUPER TEMP! this needs to move to the Blackboard if we're really doing it
    public static Transform CoreTrans;

    
    public Dictionary<int, Stat> Stats { get; }
    
    public IEnumerable<IStatHolder> GetNestedStatHolders()
    {
        throw new System.NotImplementedException();
    }
    
    protected override void Awake()
    {
        CoreTrans = transform;
        _hpStat = new Stat("Health", _hp, int.MaxValue, 0); //TEMP! Requires a config
        base.Awake();
    }

    private void OnDisable() //override OnDestroy() instead?
    {
        CoreTrans = null;
    }

    public void Heal(float amount)
    {
        _hpStat.ProcessStatModifier(new StatModifier(amount,StatusModifierType.Addition));
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(float damage, bool isCrit)
    {
        if (_hpStat.CurrentValue <= 0)
            return;
        
        _hpStat.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce));
        OnHealthChanged?.Invoke();

        if (IsEntityDead)
            StartDeathSequence();
    }

    public void StartDeathSequence()
    {
        print("GAME OVER!");
    }
}
