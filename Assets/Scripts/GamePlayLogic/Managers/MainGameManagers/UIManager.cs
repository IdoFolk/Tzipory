using System;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.BaseClass;
using Unity.VisualScripting;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class UIManager : BaseMonoObserver<IUIElement> , IInitializable , IDisposable
    {
        public void Initialize()
        {
            
        }

        public static void ShowGroup(int groupIndex)
        {
            if (ObserverObjects.TryGetValue(groupIndex, out var observers))
                observers.ForEach(observer => observer.Show());
        }
        
        public static void HideGroup(int groupIndex)
        {
            if (ObserverObjects.TryGetValue(groupIndex, out var observers))
                observers.ForEach(observer => observer.Hide());
        }

        public void Dispose()
        {
            ObserverObjects.Clear();
        }
    }
}