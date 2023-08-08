using System.Collections.Generic;
using UnityEngine;

//temp enum
public enum IndicatorCondition {AnyShamanSelected, HoverSelf, AllCall}

[System.Serializable]
public struct ProximityConfig 
{
    public List<IndicatorCondition> IndicatorConditions;
    public Color Color;
}
