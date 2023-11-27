using System;
using System.Collections.Generic;
using PathCreation;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

public class CoreTemple : BaseGameEntity, ITargetAbleEntity
{
    [SerializeField] private PathCreator _patrolPath;
    [SerializeField] private UIIndicatorConfig _uiIndicatorConfig;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [SerializeField]
    float _hp;

    [SerializeField,ReadOnly] private Stat _hpStat;
    
    private List<Enemy> _enemies = new List<Enemy>();
    
    private IObjectDisposable _uiIndicator;

    private Action CanacleFlash;
    
    public event Action<ITargetAbleEntity> OnTargetDisable;
    public bool IsTargetAble => true;

    public PathCreator PatrolPath => _patrolPath;

    public EntityType EntityType => EntityType.Core;

    public Stat InvincibleTime => throw new System.NotImplementedException();

    public bool IsDamageable => true; //temp

    public Stat Health => _hpStat;
    public bool IsEntityDead => Health.CurrentValue <= 0;


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

        _uiIndicator = UIIndicatorHandler.SetNewIndicator(CoreTrans, new UIIndicatorConfig()
        {
            Image = _spriteRenderer.sprite,
            Color = Color.white,
            AllwaysShow = false,
            DisposOnClick = false,
            OffSetRadios = 25,
            StartFlashing = false,
            FlashConfig = new UIIndicatorFlashConfig()
            {
                SizeFactor = 1.4f,
                FlashSpeed = 1.2f,
                UseTime = false,
                Time = 0,
                OverrideFlashingColor = true,
                FlashingColor = Color.red
            }
        },null,GoToCore);
        
        base.Awake();
    }

    private void GoToCore()
    {
        GameManager.CameraHandler.SetCameraPosition(transform.position);
    }

    protected override void Update()
    {
        base.Update();

        if (_enemies.Count > 0)
            CanacleFlash = UIIndicatorHandler.StartFlashOnIndicator(_uiIndicator.ObjectInstanceId);
        else
            CanacleFlash?.Invoke();
    }

    private void OnDisable() //override OnDestroy() instead?
    {
        CoreTrans = null;
    }

    public void Heal(float amount)
    {
        _hpStat.ProcessStatModifier(new StatModifier(amount,StatusModifierType.Addition),"Heal");
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(float damage, bool isCrit)
    {
        if (_hpStat.CurrentValue <= 0)
            return;
        
        _hpStat.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce),"Damage");
        OnHealthChanged?.Invoke();

        if (IsEntityDead)
            StartDeathSequence();
    }

    public void StartDeathSequence()
    {
        print("GAME OVER!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            _enemies.Remove(enemy);
        }
    }
}
