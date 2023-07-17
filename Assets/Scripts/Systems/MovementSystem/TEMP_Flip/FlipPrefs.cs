using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FlipPrefs
{
    /// <summary>
    /// Defines the +/- value of velocity.magnitude needed to "breach" the threshold and flip direction
    /// </summary>
    public float DeadZone;
    public float TestFreq;
}
