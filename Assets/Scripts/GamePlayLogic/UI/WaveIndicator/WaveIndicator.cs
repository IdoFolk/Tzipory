using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.UI.WaveIndicator
{
    public class WaveIndicator : MonoBehaviour , IInitialization
    {
        [SerializeField] private RectTransform _rotateIndicator;
        [SerializeField] private Image _timerFill;


        public bool IsInitialization { get; private set; }
        
        
        public void Init()
        {
            _timerFill.fillAmount = 1;
            IsInitialization  = true;
        }
    }
}
