using System;
using System.Collections.Generic;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Helpers;
using Tzipory.Systems.Entity;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.Totems
{
    public class Totem : BaseGameEntity
    {
        [Header("Totem Config")] [SerializeField]
        private TotemConfig _totemConfig;

        [Space] [SerializeField] private SpriteRenderer _totemSpriteRenderer;
        [SerializeField] private ProximityRingHandler _proximityRingHandler;
        [SerializeField] private ClickHelper _clickHelper;

        private List<Shaman> _shamansInsideTotemRange;
        private List<Enemy> _enemiesInsideTotemRange;
        private bool _isActive;
        private float _abilityTimer;
        private Shaman _connectedShaman;

        public Shaman ConnectedShaman => _connectedShaman;

        public TotemConfig TotemConfig => _totemConfig;
        public void Init(TotemConfig totemConfig, Shaman connectedShaman)
        {
            _totemConfig = totemConfig;
            _proximityRingHandler.Init(0, _totemConfig.Range, _totemConfig.RingColor);
            _proximityRingHandler.ToggleSprite(true);
            _connectedShaman = connectedShaman;
                
            _enemiesInsideTotemRange = new List<Enemy>();
            _shamansInsideTotemRange = new List<Shaman>();
            
            _clickHelper.SetHoldClickWaitTime(totemConfig.HoldClickWaitTime);
            _clickHelper.OnEnterHover += OnMouseEnter;
            _clickHelper.OnExitHover += OnMouseExit;
            _clickHelper.OnHoldClick += OnHoldClick;
            _proximityRingHandler.OnEnemyEnter += OnEnemyEnter;
            _proximityRingHandler.OnEnemyExit += OnEnemyExit;
            _proximityRingHandler.OnShamanEnter += OnShamanEnter;
            _proximityRingHandler.OnShamanExit += OnShamanExit;
            _abilityTimer = _totemConfig.TotemEffectInterval;
            _isActive = true;
        }

        

        private void Update() //temp
        {
            if (!_isActive) return;
            
            _abilityTimer -= GAME_TIME.GameDeltaTime;
            if (_abilityTimer <= 0)
            {
                ApplyTotemEffect();
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
            _clickHelper.OnHoldClick -= OnHoldClick;
            _clickHelper.OnEnterHover -= OnMouseEnter;
            _clickHelper.OnExitHover -= OnMouseExit;
            _proximityRingHandler.OnEnemyEnter -= OnEnemyEnter;
            _proximityRingHandler.OnEnemyExit -= OnEnemyExit;
        }

        #region Events
        private void OnHoldClick()
        {
            TotemManager.Instance.SelectTotem(EntityInstanceID,_connectedShaman.EntityInstanceID);
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

        private void ApplyTotemEffect()
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
    }
}