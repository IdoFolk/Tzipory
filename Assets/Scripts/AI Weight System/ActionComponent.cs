﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionComponent : MonoBehaviour
{
    /// <summary>
    /// The base perference to perform this action by this character
    /// </summary>
    public int baseweight; 

    public virtual List<ActionVariation> CalculateVariations()
    {
        return null;
    }
    public virtual void Action(ActionVariation av)
    {

    }
}