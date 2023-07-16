using System;
using Systems.UISystem;
using Tzipory.BaseSystem.TimeSystem;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class TimeButtonsUI : ChangeColorToggleButton
    {
        public event Action<TimeButtonsUI> OnTurnOn;
        [SerializeField] private float  _time;
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