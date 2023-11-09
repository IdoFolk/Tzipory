using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Systems.UISystem
{
    [CreateAssetMenu(fileName = "New Ui holder", menuName = "MENUNAME", order = 0)]
    public class UIGroupTagHolder : ScriptableObject
    {
        private const string TAG_PATH = "ScriptableObjects/UIGropsTag";
        
        [SerializeField] private List<UIGroupTag> _uiGroupTags;
        
        public static List<UIGroupTag> UIGroupTags { get; private set; }

        private void Awake()=>
            UIGroupTags = _uiGroupTags;

        private void OnValidate()
        {
            RefreshTags();
        }
        
        [Button("Refresh Tags")]
        private void RefreshTags()
        {
            var uiGroupTags = Resources.LoadAll<UIGroupTag>(TAG_PATH);

            foreach (var groupTag in uiGroupTags)
            {
                if (_uiGroupTags.Contains(groupTag))
                    continue;
                
                _uiGroupTags.Add(groupTag);
            }
        }
    }
}