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
        
        [Header("Camera speed")]
        [TabGroup("Camera Movement"),Tooltip("the value of the camera speed when zoom in all the way")]public float MoveSpeedMinimum = 5f;
        [TabGroup("Camera Movement"),Range(0,20),Tooltip("adds that value to the camera move speed according to how zoomed out the camera is")]public float CameraSpeedZoomChangeValue = 4f;
        [Header("Edge Scroll Detect Size")]
        [TabGroup("Camera Movement"),Range(0,0.1f),Tooltip("determines how large the edge scroll detection box is on the X")]public float EdgeScrollDetectSizeX = 0.025f;
        [TabGroup("Camera Movement"),Range(0,0.1f),Tooltip("determines how large the edge scroll detection box is on the Y")]public float EdgeScrollDetectSizeY = 0.025f;
        [Header("Camera Damping")]
        [TabGroup("Camera Movement"),Range(0,1),Tooltip("the delay of the camera that follows the target position X")]public float XDamping = 0.2f;
        [TabGroup("Camera Movement"),Range(0,1),Tooltip("the delay of the camera that follows the target position Y")]public float YDamping = 0.2f;
        [Header("Camera Damping on Event Transition")]
        [TabGroup("Camera Movement"),Range(0,1),Tooltip("the delay of the camera when transitioning to an event position X")]public float EventTransitionDampingX = 0.4f;
        [TabGroup("Camera Movement"),Range(0,1),Tooltip("the delay of the camera when transitioning to an event position Y")]public float EventTransitionDampingY = 0.4f;
        [Header("Border Padding")]
        [TabGroup("Camera Movement"),Tooltip("determines how far away the camera stays from the border on the X")]public float DefaultEdgePaddingX = 2f;
        [TabGroup("Camera Movement"),Tooltip("determines how far away the camera stays from the border on the Y")]public float DefaultEdgePaddingY = 1.3f;
        [Header("Zoom")]
        [TabGroup("Camera Zoom"),Tooltip("affects the lerp of the camera when zooming in and out")]public float ZoomSpeed = 6.5f;
        [TabGroup("Camera Zoom"),Tooltip("determines the value that will change each time you scroll up or down on the mouse wheel")]public float ZoomChangeValue = 1f;
        [TabGroup("Camera Zoom"),Tooltip("zoom starting value")]public float ZoomDefaultStartValue = 7f;
        [Header("Clamping")]
        [TabGroup("Camera Zoom"),Tooltip("the smallest value that the zoom can reach")]public float ZoomMinClamp = 2f;
        [TabGroup("Camera Zoom"),Tooltip("the largest value that the zoom can reach")]public float ZoomMaxClamp = 11f;
        
        
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