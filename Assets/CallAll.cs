using Tzipory.GameplayLogic.UI.ProximityIndicators;
using UnityEngine;

public class CallAll : MonoBehaviour
{
    public void Call()
    {
        ProximityIndicatorHandler.TEMP_CallAll_TAB?.Invoke();
    }
}
