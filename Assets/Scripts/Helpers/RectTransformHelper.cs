using UnityEngine;

namespace Helpers
{
    public static class RectTransformHelper
    {
        public static void SetScreenPointRelativeToWordPoint(this RectTransform rectTransform,Vector2 wordPos)
        {
            float minX = rectTransform.sizeDelta.x / 2;
            float minY = rectTransform.sizeDelta.y / 2;
            
            float maxX = Screen.width - minX;
            float maxY = Screen.height - minY;

            Vector2 screenPos;
            
            if (Camera.main != null)
                screenPos = Camera.main.WorldToScreenPoint(wordPos);
            else
                throw new  System.Exception("Camera.main is null");
            
            screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
            screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);

            rectTransform.position = screenPos;
        }
    }
}