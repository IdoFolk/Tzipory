using Tools.Enums;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenTimeUIHandler : BaseUIElement
    {
        [SerializeField] private  TMPro.TextMeshProUGUI _text;


        public override void UpdateUIVisual()
        {
            base.UpdateUIVisual();
            _text.text = $"{(int)(GAME_TIME.TimePlayed / 60)} : {GAME_TIME.TimePlayed % 60 :00}";
        }
    }
}