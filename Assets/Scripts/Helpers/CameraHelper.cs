using UnityEngine;

namespace Tzipory.Helpers
{
    public class CameraHelper : MonoBehaviour
    {
        private void Awake()
        {
            if (Camera.allCameras.Length > 1)
                Destroy(gameObject);
        }
    }
}