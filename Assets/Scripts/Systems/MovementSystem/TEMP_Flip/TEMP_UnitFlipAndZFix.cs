using Tzipory.SerializeData.PlayerData.Party.Entity;
using UnityEngine;

namespace Tzipory.Tools.ZFixTool
{
    public class TEMP_UnitFlipAndZFix
    {
        private const float Z_DISTANCE_MODIFIER = .5f;
        private static Vector3 CachedScaledMapSize => LevelHandler.MapSize * .01f; 

        public static float GetZForLocalPosition(Transform entity)
        {
            var position = entity.position;
            float newZ = LevelHandler.FakeForward.x * position.x + LevelHandler.FakeForward.y * position.y;
            float mapOffset = Mathf.Abs(LevelHandler.FakeForward.x) * CachedScaledMapSize.x + Mathf.Abs(LevelHandler.FakeForward.y) * CachedScaledMapSize.y;

            newZ += mapOffset;
            newZ *= Z_DISTANCE_MODIFIER;
            return -newZ;
        }
    }
}

