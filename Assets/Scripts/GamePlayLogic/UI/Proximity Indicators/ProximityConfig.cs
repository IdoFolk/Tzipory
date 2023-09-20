using System.Collections.Generic;
using UnityEngine;

//temp enum
public enum IndicatorCondition {AnyShamanSelected, HoverSelf, AllCall}

namespace Tzipory.GameplayLogic.UI.ProximityIndicators
{

    [System.Serializable]
    public struct ProximityConfig
    {
        public List<IndicatorCondition> IndicatorConditions;
        public Color Color;
    }
}