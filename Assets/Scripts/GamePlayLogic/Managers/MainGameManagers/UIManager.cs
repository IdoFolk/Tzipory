using System;
using Systems.UISystem;
using Tzipory.Tools.BaseObserver;
using Unity.VisualScripting;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityEntity.Managers
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