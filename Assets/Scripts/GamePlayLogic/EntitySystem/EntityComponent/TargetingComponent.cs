using System.Collections.Generic;
using System.Linq;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Interface;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class TargetingComponent : MonoBehaviour , ITargetableEntryReciever,ITargetableExitReciever, IEntityTargetingComponent
    {
        #region Fields

        private const string TARGETING_HANDLER_LOG_GROUP = "TargetingHandler";
        
        private List<ITargetAbleEntity> _availableTargets;

        private EntityType _targetedEntities;

        private TargetingType _targetingType;

        #endregion

        #region Proprtys

        public bool HaveTarget => CurrentTarget != null;
        public bool HaveTargetInRange => _availableTargets.Count > 0;
        public ITargetAbleEntity CurrentTarget { get; private set; }
        
        public BaseGameEntity GameEntity { get;private set;  }

        public Stat TargetingRange => Stats[(int)Constant.StatsId.TargetingRange];
        public IPriorityTargeting PriorityTargeting { get; private set; }
        public IEnumerable<ITargetAbleEntity> AvailableTargets => _availableTargets;

        public Dictionary<int, Stat> Stats { get; private set; }
        
        public bool IsInitialization { get; private set; }

        #endregion

        #region Init

        public void Init(BaseGameEntity baseGameEntity)
        {
            GameEntity = baseGameEntity;
        }

        public void Init(BaseGameEntity baseGameEntity, IInitialization<ITargetableReciever> colidierInitialization, TargetingComponentConfig componentConfig)
        {
            Init(baseGameEntity);
            
            PriorityTargeting =
                Systems.FactorySystem.ObjectFactory.TargetingPriorityFactory.GetTargetingPriority(this, componentConfig.TargetingPriorityType);
            
            _availableTargets = new List<ITargetAbleEntity>();
            
            Stats = new  Dictionary<int, Stat>()
            {
                {(int)Constant.StatsId.TargetingRange, new Stat(Constant.StatsId.TargetingRange,componentConfig.TargetingRange)}
            };

            _targetingType = componentConfig.TargetingType;
            
            _targetedEntities = componentConfig.TargetedEntity;
            
            colidierInitialization.Init(this);
            
            transform.localScale = new Vector3(TargetingRange.CurrentValue, TargetingRange.CurrentValue,1f);
            
            TargetingRange.OnValueChanged += UpdateTargetingRange;
            
            IsInitialization = true;
        }


        #endregion

        #region PublicMethod

        public void UpdateComponent()
        {
            if (_targetingType == TargetingType.Turret)
            {
                if (HaveTargetInRange)
                    TrySetNewTarget();
            }
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            return new List<IStatHolder> { this };
        }

        public float GetDistanceToTarget(ITargetAbleEntity targetAbleEntity)
        {
            return Vector2.Distance(GameEntity.EntityTransform.position, targetAbleEntity.GameEntity.transform.position);
        }

        public bool TrySetNewTarget(IPriorityTargeting priorityTargeting = null)
        {
            if (CurrentTarget is not null && CurrentTarget.EntityHealthComponent.IsEntityDead)
                CurrentTarget = null;

            if (_availableTargets.Count == 0)
                return false;

            if (priorityTargeting == null)
                CurrentTarget = PriorityTargeting.GetPriorityTarget(_availableTargets);
            else
                CurrentTarget = priorityTargeting.GetPriorityTarget(_availableTargets);

            if (!CurrentTarget.GameEntity.gameObject.activeInHierarchy)
            {
                RemoveTarget(CurrentTarget);//temp fix
                return TrySetNewTarget();
            }
            
            return true;
        }
        
        public void SetAttackTarget(ITargetAbleEntity target)=>
            CurrentTarget = target;
        
        public void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            if (!_targetedEntities.HasFlag(targetable.EntityType))
                return;
            
            Logger.Log($"Entity: <color=#de05f2>{GameEntity.name}</color>: added {targetable.GameEntity.name} to targets list",TARGETING_HANDLER_LOG_GROUP);
            
            if (!targetable.IsTargetAble)
                return;
            
            targetable.OnTargetDisable += RemoveTarget;
            _availableTargets.Add(targetable);
        }

        public void RecieveTargetableExit(ITargetAbleEntity targetable)
        {
            RemoveTarget(targetable);
        }

        public void Reset()
        {
            _availableTargets.Clear();
            TargetingRange.OnValueChanged -= UpdateTargetingRange;
        }


        #endregion

        #region PrivateMethod

        private void UpdateTargetingRange(StatChangeData statChangeData)=>
            transform.localScale = new Vector3(statChangeData.NewValue, statChangeData.NewValue,1f);
        
        private void RemoveTarget(ITargetAbleEntity targetAbleEntity)
        {
            if (_availableTargets.Contains(targetAbleEntity))
            {
                Logger.Log($"Entity: <color=#de05f2>{GameEntity.name}</color>: Remove {targetAbleEntity.GameEntity.name} from targets list entity",TARGETING_HANDLER_LOG_GROUP);
                
                targetAbleEntity.OnTargetDisable -= RemoveTarget;
                _availableTargets.Remove(targetAbleEntity);

                if (HaveTarget)
                {
                    if (targetAbleEntity.GameEntity.EntityInstanceID == CurrentTarget.GameEntity.EntityInstanceID)
                        TrySetNewTarget();
                }
            }
        }

        #endregion
    }
}