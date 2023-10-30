using System;
using Tools.Enums;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.UISystem
{
    public interface IUIElement : IInitialization
    {
        public string ElementName { get; }
        public Action OnShow { get; }
        public Action OnHide { get; }

        public UIGroup UIGroupTags { get; }
        
        void Show();
        void Hide();
        void UpdateUIVisual();
    }
}