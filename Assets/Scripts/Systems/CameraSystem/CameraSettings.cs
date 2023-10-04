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
        [Range(0,0.1f)]public float EdgeScrollDetectSizeX = 0.025f;
        [Range(0,0.1f)]public float EdgeScrollDetectSizeY = 0.025f;
        [Range(0,1)]public float XDamping = 0.2f;
        [Range(0,1)]public float YDamping = 0.2f;
        [Header("Camera Zoom")]
        public float ZoomSpeed = 6.5f;
        public float ZoomChangeAmount = 1f;
        public float ZoomMinClamp = 2f;
        public float ZoomMaxClamp = 11f;
        public float ZoomStartValue = 7f;
        [Range(0,6)]public float ZoomSpeedChangeValue = 4f;
        [Header("Camera Transition to Event")]
        [Range(0,1)]public float EventTransitionDampingX = 0.4f;
        [Range(0,1)]public float EventTransitionDampingY = 0.4f;
        [Header("Border Control")]
        public float DefaultEdgePaddingX = 2f;
        public float DefaultEdgePaddingY = 1.3f;
        
        
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