using UnityEngine;

public class PowerStructureSO : ScriptableObject
{
    public float Range;
    [Range(0,1)] public float InnerCircleRatio;
    [Range(0,1)] public float MiddleCircleRatio;
    [Range(0,1)] public float OuterCircleRatio;

}
