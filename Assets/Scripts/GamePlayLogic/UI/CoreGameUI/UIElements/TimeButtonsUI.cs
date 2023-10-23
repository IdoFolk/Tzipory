using System;
using Tools.Enums;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TimeButtonsUI : ChangeColorToggleButton
    {
        public event Action<TimeButtonsUI> OnTurnOn;
        [SerializeField] private float  _time;

        protected override UIGroupType GroupIndex => UIGroupType.GameUI;

        protected override void Awake()
        {
            UIManager.AddObserverObject(this);
        }
        
        protected override void On()
        {
            GAME_TIME.SetTimeStep(_time);
            OnTurnOn?.Invoke(this);
        }

        protected override void Off()
        {
        }
    }
}