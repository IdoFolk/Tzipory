using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Helpers
{
    public static class RectTransformHelper
    {
        public static Vector2 SetScreenPointRelativeToWordPoint(this RectTransform rectTransform,Vector2 wordPos,float offSetRadios)
        {
            var rect = rectTransform.rect;
            
            float minX = rect.width / 2;
            float minY = rect.height / 2;
            
            float maxX = Screen.width - minX;
            float maxY = Screen.height - minY;

            Vector2 screenPos = GameManager.CameraHandler.MainCamera.WorldToScreenPoint(wordPos);
            
            screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
            screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);

            if (Vector2.Distance(screenPos, rectTransform.position) < offSetRadios)
                return screenPos;

            rectTransform.position = screenPos;

            return screenPos;
        }
    }
}