using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameplayLogic.UI.HPBar;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Helpers;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.GameplayLogic.EntitySystem.Totems
{
    public class Totem : BaseGameEntity, IEntityTargetAbleComponent
    {
        [Header("Totem Config")] [SerializeField]
        private TotemConfig _totemConfig;

        [Space] [SerializeField] private SpriteRenderer _totemSpriteRenderer;
        [SerializeField] private SpriteRenderer _loadingcircleSpriteRenderer;
        [SerializeField] private ProximityRingHandler _proximityRingHandler;
        [SerializeField] private ClickHelper _clickHelper;
        [SerializeField] private TEMP_UNIT_HPBarConnector _hpBarConnector;

        private List<Shaman> _shamansInsideTotemRange;
        private List<Enemy> _enemiesInsideTotemRange;
        private bool _isActive;
        private float  _currentInvincibleTime;
        private bool _startedDeathSequence;

        private float _abilityTimer;
        private Shaman _connectedShaman;
        public Dictionary<int, Stat> Stats { get; private set; }        
        public StatHandler StatHandler { get; }
        public EntityType EntityType { get; private set; }
        public bool IsTargetAble { get; private set; }
        public TEMP_UNIT_HPBarConnector HpBarConnector => _hpBarConnector;
        public Shaman ConnectedShaman => _connectedShaman;

        public TotemConfig TotemConfig => _totemConfig;
        public Stat Health  
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.Health, out var health))
                    return health;
                
                throw new Exception($"{Constant.StatsId.Health} not found in entity {GameEntity.name}");
            }
        }
        public Stat InvincibleTime  {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.InvincibleTime, out var invincibleTime))
                    return invincibleTime;
                
                throw new Exception($"{Constant.StatsId.InvincibleTime} not found in entity {GameEntity.name}");
            }
        }
        public void Init(TotemConfig totemConfig, Shaman connectedShaman)
        {
            _totemConfig = totemConfig;
            _proximityRingHandler.Init(0, _totemConfig.Range, _totemConfig.RingColor);
            _proximityRingHandler.ToggleSprite(true);
            _connectedShaman = connectedShaman;
            EntityType = EntityType.Totem;
            Stats = new Dictionary<int, Stat>();
            foreach (var statConfig in _totemConfig.TotemStatConfigs)
            {
                var stat = new Stat(statConfig);
                Stats.Add((int)statConfig._statsId,stat);
            }
            _hpBarConnector.Init(Health.BaseValue);

            _enemiesInsideTotemRange = new List<Enemy>();
            _shamansInsideTotemRange = new List<Shaman>();


            Health.OnValueChanged += _hpBarConnector.SetBarToHealth;
            _clickHelper.SetHoldClickWaitTime(totemConfig.HoldClickWaitTime);
            _clickHelper.OnEnterHover += OnMouseEnter;
            _clickHelper.OnExitHover += OnMouseExit;
            _clickHelper.OnHoldClickFinish += OnHoldClickFinish;
            _clickHelper.OnHoldClickStart += OnHoldClickStart;
            _proximityRingHandler.OnEnemyEnter += OnEnemyEnter;
            _proximityRingHandler.OnEnemyExit += OnEnemyExit;
            _proximityRingHandler.OnShamanEnter += OnShamanEnter;
            _proximityRingHandler.OnShamanExit += OnShamanExit;
            _abilityTimer = _totemConfig.TotemEffectInterval;
            _isActive = true;
            IsTargetAble = true;
            IsDamageable = true;
        }


        protected override void Update()
        {
            base.Update();
            HealthComponentUpdate();
            if (IsEntityDead)
            {
                if (_startedDeathSequence)
                    return;
                
                StartDeathSequence();
                return;
            }
            if (!_isActive) return;

            _abilityTimer -= GAME_TIME.GameDeltaTime;
            if (_abilityTimer <= 0)
            {
                ApplyTotemAbility();
                _abilityTimer = _totemConfig.TotemEffectInterval;
            }
        }

        private void OnValidate()
        {
            _totemSpriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
            _proximityRingHandler ??= GetComponentInChildren<ProximityRingHandler>();
            _clickHelper ??= GetComponentInChildren<ClickHelper>();
        }

        private void OnDestroy()
        {
            Health.OnValueChanged -= _hpBarConnector.SetBarToHealth;
            _clickHelper.OnHoldClickFinish -= OnHoldClickFinish;
            _clickHelper.OnHoldClickStart -= OnHoldClickStart;
            _clickHelper.OnEnterHover -= OnMouseEnter;
            _clickHelper.OnExitHover -= OnMouseExit;
            _proximityRingHandler.OnEnemyEnter -= OnEnemyEnter;
            _proximityRingHandler.OnEnemyExit -= OnEnemyExit;
        }

        private void ApplyTotemAbility()
        {
            switch (_totemConfig.TotemEffectUnitType)
            {
                case TotemEffectUnitType.Enemy:
                    foreach (var enemy in _enemiesInsideTotemRange)
                    {
                        enemy.StatHandler.AddStatEffect(_totemConfig.StatEffectConfig);
                    }

                    break;
                case TotemEffectUnitType.Shaman:
                    foreach (var shaman in _shamansInsideTotemRange)
                    {
                        shaman.StatHandler.AddStatEffect(_totemConfig.StatEffectConfig);
                    }

                    break;
            }
        }

        #region Events

        private void OnHoldClickStart()
        {
            _loadingcircleSpriteRenderer.gameObject.SetActive(true);
            Cursor.visible = false;
            StartCoroutine(LoadingCircle());
        }


        private void OnHoldClickFinish()
        {
            _loadingcircleSpriteRenderer.gameObject.SetActive(false);
            TotemManager.Instance.SelectTotem(_connectedShaman.EntityInstanceID);
        }

        private void OnMouseEnter()
        {
        }

        private void OnMouseExit()
        {
        }

        private void OnEnemyEnter(int id, Enemy enemy)
        {
            _enemiesInsideTotemRange.Add(enemy);
        }

        private void OnEnemyExit(int id, Enemy enemy)
        {
            _enemiesInsideTotemRange.Remove(enemy);
        }

        private void OnShamanEnter(int id, Shaman shaman)
        {
            _shamansInsideTotemRange.Add(shaman);
        }

        private void OnShamanExit(int id, Shaman shaman)
        {
            _shamansInsideTotemRange.Remove(shaman);
        }

        #endregion

        IEnumerator LoadingCircle()
        {
            while (true)
            {
                var circleRatio = 360 / _totemConfig.HoldClickWaitTime;
                var radius = _clickHelper.HoldClickTimer * circleRatio;
                _loadingcircleSpriteRenderer.material.SetFloat("_Arc2", radius);
                yield return new WaitUntil(() => _clickHelper.HoldClickTimerActive);
            }
        }

        
        
        public bool IsDamageable { get; private set; }
        public bool IsEntityDead => Health.CurrentValue <= 0;
        
        public void Heal(float amount)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(float damage, bool isCrit)
        {
            if (!IsDamageable) return;
            //EffectSequenceHandler.PlaySequenceById(isCrit
            //     ? Constant.EffectSequenceIds.GET_CRIT_HIT
            //    : Constant.EffectSequenceIds.GET_HIT);
                
            PopUpTextConfig popUpTextConfig;
            string processName;

            if (isCrit)
            {
                popUpTextConfig = PopUpTextManager.Instance.GetCritHitDefaultConfig;
                processName = "Crit Hit";
            }
            else
            {
                popUpTextConfig = PopUpTextManager.Instance.GetHitDefaultConfig;
                processName = "Hit";
            }

            Health.ProcessStatModifier(new StatModifier(damage,StatusModifierType.Reduce),processName,popUpTextConfig);
        }
        private void HealthComponentUpdate()
        {
            if (IsDamageable) return;
            _currentInvincibleTime -= GAME_TIME.GameDeltaTime;

            if (!(_currentInvincibleTime < 0)) return;
            IsDamageable = true;
            _currentInvincibleTime = InvincibleTime.CurrentValue;

        }
        public void StartDeathSequence()
        {
            _startedDeathSequence = true;
            
            IsTargetAble = false;
            IsDamageable = false;
                
            OnTargetDisable?.Invoke(this);
            //EffectSequenceHandler.PlaySequenceById(Constant.EffectSequenceIds.DEATH,EntityDied);
            gameObject.SetActive(false);
        }

        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IStatHolder[] statHolders = { this };
            return statHolders;
        }
        public event Action<IEntityTargetAbleComponent> OnTargetDisable;
        
    }
}