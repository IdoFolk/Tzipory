using System;
using System.Collections;
using System.Collections.Generic;
using GameplayLogic.UI.HPBar;
using PathCreation;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.FactorySystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

public class CoreTemple : BaseGameEntity, ITargetAbleEntity , IInitialization
{
    [SerializeField] private PathCreator _patrolPath;
    [SerializeField] private UIIndicatorConfig _uiIndicatorConfig;
    [SerializeField] private SpriteRenderer coreSpriteRenderer;
    [SerializeField] private Transform brokenCoreSpriteRenderer;
    [SerializeField] private SpriteRenderer cracksSpriteRenderer;
    [SerializeField] private ParticleSystem cracksGlowParticleSystem;
    [SerializeField] private TEMP_HP_Bar _hpBar;
    [SerializeField] private Animator _coreAnimator;
    [SerializeField] private float hp;
    
    private readonly List<UnitEntity> _enemies = new();
    
    private IObjectDisposable _uiIndicator;

    private Action _canacleFlash;

    public static Transform CoreTransform { get; private set; }        
    
    public event Action<ITargetAbleEntity> OnTargetDisable;
    
    public bool IsTargetAble => true;

    public PathCreator PatrolPath => _patrolPath;

    public EntityType EntityType => EntityType.Core;
    public bool IsDestroyed { get; private set; }
    public IEntityHealthComponent EntityHealthComponent { get; private set; }
    public IEntityStatComponent EntityStatComponent { get; }//not in use
    public IEntityVisualComponent EntityVisualComponent { get; }// not in use
    
    public bool IsInitialization { get; private set; }
    
    public void Init()
    {
        IsDestroyed = false;
        
        CoreTransform = transform;
        
        var config = new HealthComponentConfig()
        {
            HealthComponentType = HealthComponentType.Standard,
            HealthStat = hp,
        };
        
        EntityHealthComponent = HealthComponentFactory.GetHealthComponent(config);
        AddComponent(EntityHealthComponent);
        EntityHealthComponent.Init(this,config);
        
        _hpBar.Init(EntityHealthComponent.Health.BaseValue,EntityType);
        
        EntityHealthComponent.Health.OnValueChanged += OnHealthChanage;
        
        _uiIndicator = UIIndicatorHandler.SetNewIndicator(transform, new UIIndicatorConfig()
        {
            Image = coreSpriteRenderer.sprite,
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

        IsInitialization = true;
        StartCoroutine(nameof(UpdateUIIndicator));
    }

    private void OnHealthChanage(StatChangeData statChangeData)
    {
        _hpBar.SetBarValue(statChangeData.NewValue);
        if (statChangeData.NewValue / EntityHealthComponent.Health.BaseValue < 0.6667f)
        {
            _coreAnimator.SetBool("R_Crack",true);
        }
        if (statChangeData.NewValue / EntityHealthComponent.Health.BaseValue < 0.3336f)
        {
            _coreAnimator.SetBool("L_Crack",true);
        }
        if (statChangeData.NewValue / EntityHealthComponent.Health.BaseValue <= 0)
        {
            GameManager.CameraHandler.SetCameraPosition(transform.position);
            GameManager.CameraHandler.SetCameraZoom(4);
            _hpBar.gameObject.SetActive(false);
            if (!cracksGlowParticleSystem.isPlaying)
                cracksGlowParticleSystem.Play();
            Invoke(nameof(DestroyCoreAnimation),2);
        }
    }

    private void DestroyCoreAnimation()
    {
        brokenCoreSpriteRenderer.gameObject.SetActive(true);
        coreSpriteRenderer.enabled = false;
        cracksSpriteRenderer.enabled = false;
        cracksGlowParticleSystem.gameObject.SetActive(false);
        _coreAnimator.SetBool("Break",true);
        Invoke(nameof(EndGame),2);
    }

    private void EndGame()
    {
        IsDestroyed = true;
    }
    private void GoToCore()
    {
        GameManager.CameraHandler.SetCameraPosition(transform.position);
    }

    private IEnumerable UpdateUIIndicator()
    {
        if (_enemies.Count > 0)
            _canacleFlash = UIIndicatorHandler.StartFlashOnIndicator(_uiIndicator.ObjectInstanceId);
        else
            _canacleFlash?.Invoke();

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<UnitEntity>(out var enemy))
            _enemies.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<UnitEntity>(out var enemy))
            _enemies.Remove(enemy);
    }

    private void OnDestroy()
    {
        EntityHealthComponent.Health.OnValueChanged -= OnHealthChanage;
    }
}
