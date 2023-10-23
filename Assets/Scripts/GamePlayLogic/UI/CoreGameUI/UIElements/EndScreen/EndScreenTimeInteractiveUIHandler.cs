using Tools.Enums;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UIElements.EndScreen
{
    public class EndScreenTimeInteractiveUIHandler : BaseUIElement
    {
        [SerializeField] private  TMPro.TextMeshProUGUI _text;

        protected override UIGroupType UIGroup => UIGroupType.EndGameUI;

        public override void Show()
        {
            _text.text = $"{(int)(GAME_TIME.TimePlayed / 60)} : {GAME_TIME.TimePlayed % 60 :00}";
            base.Show();
        }
    }
}