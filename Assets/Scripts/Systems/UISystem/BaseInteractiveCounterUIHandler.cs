using System.Globalization;
using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public abstract class BaseInteractiveCounterUIHandler : BaseInteractiveUIElement
    {
        [SerializeField] protected TMP_Text _currentCount;
        [SerializeField] protected TMP_Text _maxCount;
        
        
        protected void UpdateUiData(float currentCunt)
        {
            _currentCount.text = currentCunt.ToString(CultureInfo.CurrentCulture);
        }
        
        protected void UpdateUiData(int currentCunt)
        {
            _currentCount.text = currentCunt.ToString();
        }
    }
}