using Tzipory.SerializeData.PlayerData.Party.Entity;
using UnityEngine;

namespace Tzipory.Tools.ZFixTool
{
    public class TEMP_UnitFlipAndZFix
    {
        private const float Z_DISTANCE_MODIFIER = 10;

        public static float GetZForLocalPosition(Transform entity)
        {
            var position = GetPositionReletveToMap(entity.position);
            
            float newZ = LevelHandler.FakeForward.x * position.x + LevelHandler.FakeForward.y * position.y;

            //newZ -= Z_DISTANCE_MODIFIER;
            return -newZ;
        }

        private static Vector2 GetPositionReletveToMap(Vector2 position) =>
            new(position.x - LevelHandler.MapStartWordPosition.x, position.y - LevelHandler.MapStartWordPosition.y);
        
    }
}