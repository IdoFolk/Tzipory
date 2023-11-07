using UnityEngine;

namespace Tzipory.Helpers
{
    public static class TransformHelper
    {
        public static bool InVisibleOnScreen(this Transform transform)
        {
            var position = transform.position;
            
            if (position.x < 0 || position.x > Screen.width)
                return false;

            if (position.y < 0 || position.y > Screen.height)
                return false;
            
            return true;
        }
    }
}