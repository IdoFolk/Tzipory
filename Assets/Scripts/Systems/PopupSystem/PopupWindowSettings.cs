using UnityEngine;

namespace Tzipory.Systems.PopupSystem
{
    [CreateAssetMenu(fileName = "NewPopupWindowSettings", menuName = "ScriptableObjects/PopupWindow")]
    public class PopupWindowSettings : ScriptableObject
    {
        [Header("Window Position Padding")] public Vector2 PositionPadding = new Vector2(0, 0);

        public Vector2 WindowSize = new Vector2(250, 170);
        public float HeaderFontSize = 24f;
        public float BodyFontSize = 16f;


    }
}