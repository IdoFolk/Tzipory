using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_VisualCruves : MonoBehaviour
{
    public static AnimationCurve PopUpText_MoveCurve;
    public static AnimationCurve PopUpText_ScaleCurve;
    public static AnimationCurve PopUpText_AlphaCurve;

    [SerializeField] AnimationCurve _moveCurve;
    [SerializeField] AnimationCurve _scaleCurve;
    [SerializeField] AnimationCurve _alphaCurve;
    void Awake()
    {
        PopUpText_MoveCurve = _moveCurve;
        PopUpText_ScaleCurve = _scaleCurve;
        PopUpText_AlphaCurve = _alphaCurve;
    }
}
