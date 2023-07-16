using Systems.UISystem;
using TMPro;
using UnityEngine;

namespace GameplayeLogic.UIElements
{
    public abstract class BaseCounterUIHandler : BaseUIElement
    {
        [SerializeField] protected TMP_Text _currentCount;
        [SerializeField] protected TMP_Text _maxCount;
        
        
        protected void UpdateUiData(float currentCunt)
        {
            _currentCount.text = currentCunt.ToString();
        }
        
        protected void UpdateUiData(int currentCunt)
        {
            _currentCount.text = currentCunt.ToString();
        }
    }
}