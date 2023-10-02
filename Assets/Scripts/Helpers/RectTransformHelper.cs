using System;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

namespace Tzipory.Helpers
{
    public static class RectTransformHelper
    {
        public static void SetScreenPointRelativeToWordPoint(this RectTransform rectTransform,Vector2 wordPos,Vector2 offSet)
        {
            var rect = rectTransform.rect;
            
            float minX = rect.width / 2;
            float minY = rect.height / 2;
            
            float maxX = Screen.width - minX;
            float maxY = Screen.height - minY;

            Vector2 screenPos;
            
            if (GameManager.CameraHandler == null)
            {
                if (Camera.main != null) 
                    screenPos = Camera.main.WorldToScreenPoint(wordPos);
                else
                    throw new Exception("No camera was found");
            }
            else
                screenPos = GameManager.CameraHandler.MainCamera.WorldToScreenPoint(wordPos);

            screenPos += offSet;
                
            screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
            screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);

            rectTransform.position = screenPos;
        }
    }
}