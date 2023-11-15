using System.Collections.Generic;
using Tzipory.Systems.SaveLoadSystem.SaveSystemJson;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Tzipory.Tools.Debag
{
    [InitializeOnLoad]
    public class Logger
    {
        private static readonly string LOGGroupPath = $"{Application.dataPath}/GameSetting/LogGroups";
        
        public static readonly Dictionary<string, LogGroup> LogGroups;
        
        static Logger()
        {
            if (GameSaveUtilityJson.LoadObjects(LOGGroupPath,out IEnumerable<LogGroup> saveData))
            {
                LogGroups = new  Dictionary<string, LogGroup>();

                foreach (var logGroup in saveData)
                    LogGroups.Add(logGroup.Name,logGroup);                   
               
                Debug.Log("Load log group");
            }
            else
            {
                LogGroups = new  Dictionary<string, LogGroup>();
            }
        }

        public static void DeleteGroup(LogGroup logGroup)
        {
            if (!LogGroups.TryGetValue(logGroup.Name,out var group))
                throw  new System.Exception($"Log group {logGroup.Name} not found!");
                    
            LogGroups.Remove(logGroup.Name);

            GameSaveUtilityJson.DeleteKey($"{LOGGroupPath}/{group.Name}.json");
            GameSaveUtilityJson.DeleteKey($"{LOGGroupPath}/{group.Name}.json.meta");
        }

        public static void SaveLogData()
        {
            GameSaveUtilityJson.SaveObjects(LOGGroupPath,LogGroups.Values);
        }

        public static void Log(object message, string groupName = null)
        {
#if UNITY_EDITOR
            if (groupName == null)
            {
                Debug.Log(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                //group not found!
                throw new System.Exception($"Log group {groupName} not found! from object {message.GetType().Name}");
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.Log(ProcessMessage(message, logGroup));
#endif
        }
       
        public static void LogWarning(object message, string groupName = null)
        {
#if UNITY_EDITOR
            if (groupName == null)
            {
                Debug.LogWarning(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                //group not found!
                throw new System.Exception($"Log group {groupName} not found! from object {message.GetType().Name}");
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.LogWarning(ProcessMessage(message, logGroup));
#endif
        }

        public static void LogError(object message, string groupName = null)
        {
#if UNITY_EDITOR
            if (groupName == null)
            {
                Debug.LogError(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                //group not found!
                throw new System.Exception($"Log group {groupName} not found! from object {message.GetType().Name}");
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.LogError(ProcessMessage(message, logGroup));
#endif
        }

        private static string ProcessMessage(object message, LogGroup logGroup)
        {
            return logGroup == null ? message.ToString() : $"<color=#{logGroup.Color.ToHexString()}>{logGroup.Name}:</color> : {message}";
        }
    }
}