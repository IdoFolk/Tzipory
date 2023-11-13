using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Helpers
{
    public static class RectTransformHelper
    {
        public static Vector2 SetScreenPointRelativeToWordPoint(this RectTransform rectTransform,Vector2 wordPos,float offSet)
        {
            var rect = rectTransform.rect;
            
            float minX = rect.width / 2;
            float minY = rect.height / 2;
            
            float maxX = Screen.width - minX;
            float maxY = Screen.height - minY;
            
            Vector2 screenPos = GameManager.CameraHandler.MainCamera.WorldToScreenPoint(wordPos);
            
            Vector2 clampScreenPoint =
                new Vector2(Mathf.Clamp(screenPos.x, minX, maxX), Mathf.Clamp(screenPos.y, minY, maxY));
            
            var duration = clampScreenPoint - new Vector2(Screen.width / 2, Screen.height / 2);
            
            Vector2 closestPoint = clampScreenPoint - duration.normalized * offSet;
            
            rectTransform.anchoredPosition = closestPoint;
            
            return screenPos;
        }
    }
}