using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tzipory.Systems.CameraSystem
{
    public class CameraSettings : ScriptableObject
    {
        private const string CAMERA_SETTING_PATH = "/CameraSettings";
        
        [Header("Camera Movement")]
        public float MoveSpeed = 10f;
        public int EdgeScrollDetectSize = 20;
        [Range(0,1)]public float XDamping = 0.2f;
        [Range(0,1)]public float YDamping = 0.2f;
        [Header("Camera Zoom")]
        public float ZoomSpeed = 8f;
        public float ZoomAmount = 1f;
        public float ZoomMin = 2f;
        public float ZoomMax = 8f;
        public float ZoomStartValue;
        [Header("Border Control")]
        public float EdgeDampingX = 2f;
        public float EdgeDampingY = 1.3f;
        
        [Button("Set as camera setting")] //WIP (not working)
        public void SetAsCameraSetting()
        {
            CameraHandler.CameraSettings = this;
            Debug.Log($"Set {name} as the active camera setting");
        }
        
        [MenuItem("Game Setting/New camera settings")]
        public static void CreateNewCameraSetting()
        {
            var cameraSetting = CreateInstance<CameraSettings>();
            
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo($"{Application.dataPath}{CAMERA_SETTING_PATH}");
            int count = dir.GetFiles().Length / 2;
            
            cameraSetting.name = $"CameraSetting{count + 1}";
            
            AssetDatabase.CreateAsset(cameraSetting,$"Assets/{CAMERA_SETTING_PATH}/{cameraSetting.name}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}