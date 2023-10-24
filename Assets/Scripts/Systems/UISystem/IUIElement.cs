using System;

namespace Tzipory.Systems.UISystem
{
    public interface IUIElement
    {
        public string ElementName { get; }
        public Action OnShow { get; }
        public Action OnHide { get; }
        
        void Show();
        void Hide();
        void UpdateUIVisual();
    }

    public enum UIGroup
    {
        GameUI,
        PauseUI,
        EndGameUI,
        MainMenuUI,
        SettingsUI,
        LoadingUI,
    }
}