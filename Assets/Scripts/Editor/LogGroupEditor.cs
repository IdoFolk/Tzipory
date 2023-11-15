using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Tzipory.Systems.SaveLoadSystem.SaveSystemJson;
using Tzipory.Tools.Debag;
using UnityEditor;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Editor
{
    public class LogGroupEditor : OdinMenuEditorWindow
    {
        private static readonly string LOGGroupPath = $"{Application.dataPath}/GameSetting/LogGroups";
        
        private LogGroupSerialize _newLogGroupSerialize;
        
        private List<LogGroup> _logGroups;
        
        [MenuItem("Tools/Log Editor")]
        private static void OpenWindows()
        {
            var window = GetWindow<LogGroupEditor>();

            // if (GameSaveUtilityJson.LoadObjects($"{Application.dataPath}/GameSetting/LogGroups",out IEnumerable<LogGroup> saveData))
            // {
            //     Logger.LogGroups = saveData.ToList();
            // }
            
            window.position = new Rect(200,200,1000,1000);
            window.Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            _newLogGroupSerialize = new LogGroupSerialize(CreateNewLogGroup);
            
            tree.Add("Create log group",_newLogGroupSerialize);
            
            foreach (var logGroup in Logger.LogGroups.Values)
            {
                var logGroupGroupName = logGroup.Name ?? "";
                
                tree.Add(logGroupGroupName, logGroup);
            }   
            
            return tree;
        }
        
        private void CreateNewLogGroup(LogGroup logGroup)
        {
            Logger.LogGroups.Add(logGroup.Name,logGroup);
            GameSaveUtilityJson.SaveObject(LOGGroupPath,logGroup);
            EditorUtility.SetDirty(this);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Logger.SaveLogData();
            
            if (_newLogGroupSerialize is not null)
                _newLogGroupSerialize = null;
        }
        
        [System.Serializable]
        private class LogGroupSerialize
        {
            private Action<LogGroup> _onSave;

            [SerializeField] private string _name = "New Log Group";
            [SerializeField] private Color _color;
            
            public LogGroupSerialize(Action<LogGroup> onSave)
            {
                _onSave = onSave;
            }

            [Button]
            private void Save()
            {
                if (_name is null || _name.Length == 0)
                {
                    Debug.LogError($"Log group as no Name!");
                    return;
                }
                
                LogGroup  LOGGroup = new LogGroup()
                {
                    Name = _name,
                    Color = _color
                };
                
                _onSave.Invoke(LOGGroup);
            }
        }
    }

    
}