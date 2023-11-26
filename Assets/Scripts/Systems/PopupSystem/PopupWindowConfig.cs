using Tzipory.ConfigFiles;
using UnityEngine;

namespace Tzipory.Systems.PopupSystem
{
    [System.Serializable]
    public class PopupWindowConfig : IConfigFile
    {
        public Vector2 PositionPadding = new Vector2(0, 0);
        public Vector2 WindowSize = new Vector2(250, 170);
        public float HeaderFontSize = 24f;
        public float BodyFontSize = 16f;
        public string HeaderText; //Temp
        public string BodyText; //Temp


        public int ObjectId { get; }
        public int ConfigTypeId { get; }
    }
}