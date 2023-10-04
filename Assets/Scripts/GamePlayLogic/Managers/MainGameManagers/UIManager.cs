using System;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.BaseClass;
using Unity.VisualScripting;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class UIManager : BaseObserver<IUIElement> , IInitializable , IDisposable
    {
        public void Initialize()
        {
            for (int i = 0; i < ObserverObjects.Count; i++)
            {
                ObserverObjects[i].Show();
            }
        }

        public void Dispose()
        {
            ObserverObjects.Clear();
        }
    }
}