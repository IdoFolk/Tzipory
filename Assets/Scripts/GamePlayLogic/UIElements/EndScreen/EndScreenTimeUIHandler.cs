using Systems.UISystem;
using Tzipory.BaseSystem.TimeSystem;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public class EndScreenTimeUIHandler : BaseUIElement
    {
        [SerializeField] private  TMPro.TextMeshProUGUI _text;

        public override void Show()
        {
            _text.text = $"{(int)(GAME_TIME.TimePlayed / 60)} : {GAME_TIME.TimePlayed % 60 :00}";
            base.Show();
        }
    }
}