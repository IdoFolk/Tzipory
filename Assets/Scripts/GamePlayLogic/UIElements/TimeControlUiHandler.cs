using System.Collections.Generic;
using Systems.UISystem;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class TimeControlUiHandler : BaseUIElement
    {
        [SerializeField] private List<TimeButtonsUI> _timeButtons;
        
        private TimeButtonsUI  _currentButton;
        
        private void Start()
        {
            foreach (var timeButtonsUI in _timeButtons)
                timeButtonsUI.OnTurnOn  += OnButtonPressed;
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