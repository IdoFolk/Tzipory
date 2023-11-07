using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Systems.UISystem
{
    public class UIIndicator : BaseInteractiveUIElement , IInitialization<UIIndicatorConfig,Action>
    {
        private Vector2 _refPosition;
        private Vector2 _offSet;

        public void Init(UIIndicatorConfig config, Action onCompleted = null)
        {
            _refPosition = config.RefPoint;
            _offSet = config.OffSet;
        }
        
        private void Update()
        {
            if (!IsInitialization)
                return;
            
        }
    }
}