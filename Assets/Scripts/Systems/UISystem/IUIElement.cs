using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.UISystem
{
    public interface IUIElement : IInitialization
    {
        public string ElementName { get; }
        public UIGroup UIGroupTags { get; }
        
        void Show();
        void Hide();
        void UpdateUIVisual();
    }
}