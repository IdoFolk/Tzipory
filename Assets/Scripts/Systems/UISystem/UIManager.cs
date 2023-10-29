using System.Collections.Generic;
using Tools.Enums;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
     public class UIManager : MonoBehaviour
    {
        private static readonly Dictionary<UIGroup, List<IUIElement>> UIGroups = new();
        
        public static void Init(UIGroup parameter)
        {
            if (!UIGroups.TryGetValue(parameter, out var uiElements))
            {
                Debug.LogWarning($"Can not find UiGroup in tag {parameter}");
                return;
            }

            for (int i = 0; i < uiElements.Count; i++)
                uiElements[i].Init();
        }

        public static void Init()
        {
            foreach (var uiGroupsValue in UIGroups.Values)
            {
                foreach (var uiElement in uiGroupsValue)
                    uiElement.Init();
            }
        }

        public static void AddUIElement(UIGroup group, IUIElement element)
        {
            if (UIGroups.TryGetValue(group,out var foundUIGroup))
            {
                if (foundUIGroup.Contains(element))
                {
                    Debug.LogWarning("UiElement already exists in this group ");
                    return;
                }
                
                foundUIGroup.Add(element);
            }
            else
            {
                UIGroups.Add(group, new List<IUIElement>(){element});
            }
        }
        
        public static void RemoveUIElement(IUIElement element)
        {
            foreach (var uiElements in UIGroups.Values)
            {
                if (!uiElements.Contains(element)) continue;
                
                uiElements.Remove(element);
                return;
            }
            
            Debug.LogWarning("UiElement not found in any group");
        }
        
        public static void ShowUIGroup(UIGroup group,bool updateOnShow = false)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                foreach (var uiElement in foundUIGroup)
                {
                    uiElement.Show();
                    if (updateOnShow)
                        uiElement.UpdateUIVisual();
                }
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }
        
        public static void HidUIGroup(UIGroup group)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                foreach (var uiElement in foundUIGroup)
                    uiElement.Hide();
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }

        public static void UpdateVisualUIGroup(UIGroup group)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                foreach (var uiElement in foundUIGroup)
                    uiElement.UpdateUIVisual();
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }
    }
}