using System.Collections.Generic;
using Tools.Enums;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements
{
    public class TimeControlUIHandler : BaseUIElement
    {
        [SerializeField] private List<TimeButtonsUI> _timeButtons;
        
        private TimeButtonsUI  _currentButton;

        protected override UIGroup UIGroup => UIGroup.GameUI;
        
        public override void Show()
        {
            foreach (var timeButtonsUI in _timeButtons)
            {
                timeButtonsUI.OnTurnOn  += OnButtonPressed;
                if(timeButtonsUI.State == ButtonState.On)
                    _currentButton = timeButtonsUI;
            }
            base.Show();
        }
        
        
        public override void Hide()
        {
            foreach (var timeButtonsUI in _timeButtons)
                timeButtonsUI.OnTurnOn  -= OnButtonPressed;
            base.Hide();
        }

        private void OnButtonPressed(TimeButtonsUI timeButtonsUI)
        {
            if (_currentButton == null)
            {
                _currentButton = timeButtonsUI;
                return;
            }
            
            _currentButton.ChangeState(ButtonState.Off);
            _currentButton = timeButtonsUI;
        }
    }
}