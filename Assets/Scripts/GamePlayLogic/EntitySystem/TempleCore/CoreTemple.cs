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
using Unity.VisualScripting;
using UnityEngine;

public class CoreTemple : BaseGameEntity, IEntityTargetAbleComponent
{
    [SerializeField] private PathCreator _patrolPath;
    [SerializeField] private UIIndicatorConfig _uiIndicatorConfig;
    [SerializeField] private SpriteRenderer _defaultCoreSpriteRenderer;
    [SerializeField] private GameObject _brokenCoreSpriteRenderer;
    [SerializeField] private ParticleSystem _glowParticleSystem;
    [SerializeField] private Animator _coreAnimator;
    
    [SerializeField] float _hp;
    [SerializeField] float _deathAnimationTime;
    [SerializeField] int _animationCameraZoom;

    [SerializeField,ReadOnly] private Stat _hpStat;
    
    private List<Enemy> _enemies = new List<Enemy>();
    
    private IObjectDisposable _uiIndicator;

    private Action CanacleFlash;
    private bool _leftCrackAnimationHappened;
    private bool _rightCrackAnimationHappened;
    private bool _BreakAnimationHappened;
    
    public event Action<IEntityTargetAbleComponent> OnTargetDisable;
    public bool IsTargetAble => true;

    public PathCreator PatrolPath => _patrolPath;

    public EntityType EntityType => EntityType.Core;

    public Stat InvincibleTime => throw new System.NotImplementedException();

    public bool IsDamageable => true; //temp

    public Stat Health => _hpStat;

    public StatHandler StatHandler => throw new System.NotImplementedException();

    public System.Action OnHealthChanged;

    //SUPER TEMP! this needs to move to the Blackboard if we're really doing it
    public static Transform CoreTrans;

    public bool IsEntityDead { get; private set; }
    
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
            Image = _defaultCoreSpriteRenderer.sprite,
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
        if (_hpStat.CurrentValue <= 0 && !_BreakAnimationHappened)
        {
            _brokenCoreSpriteRenderer.SetActive(true);
            _defaultCoreSpriteRenderer.enabled = false;
            GameManager.CameraHandler.SetCameraPosition(transform.position);
            GameManager.CameraHandler.LockCameraWithEase(transform.position,_animationCameraZoom);
            _glowParticleSystem.gameObject.SetActive(true);
            _glowParticleSystem.Play();
            Invoke(nameof(DeathAnimation),_deathAnimationTime);
            _BreakAnimationHappened = true;
            return;
        }
        var hpRatio = _hpStat.CurrentValue / _hpStat.BaseValue;
        if (hpRatio <= 0.6667 && !_rightCrackAnimationHappened)
        {
            _coreAnimator.SetBool("R_Crack",true);
            _rightCrackAnimationHappened = true;
        }
        if (hpRatio <= 0.3336 && !_leftCrackAnimationHappened)
        {
            _coreAnimator.SetBool("L_Crack",true);
            _leftCrackAnimationHappened = true;
        }
       
        
        _hpStat.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce),"Damage");
        OnHealthChanged?.Invoke();

    }

    public void StartDeathSequence()
    {
        print("GAME OVER!");
        IsEntityDead = true;
    }

    private void DeathAnimation()
    {
        _coreAnimator.SetBool("Break",true);
        _glowParticleSystem.gameObject.SetActive(false);
        Invoke(nameof(StartDeathSequence), _deathAnimationTime);
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
