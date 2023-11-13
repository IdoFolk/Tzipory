using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Helpers
{
    public static class TransformHelper
    {
        public static bool InVisibleOnScreen(this Transform transform)
        {
            var screenPosition = GameManager.CameraHandler.MainCamera.WorldToScreenPoint(transform.position); 
            
            if (screenPosition.x < 0 || screenPosition.x > Screen.width)
                return false;

            if (screenPosition.y < 0 || screenPosition.y > Screen.height)
                return false;
            
            return true;
        }
    }
}