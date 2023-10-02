using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tzipory.Helpers;
using Tzipory.Testing;

public class LevelVisualData_Monoton : MonoSingleton<LevelVisualData_Monoton> //TODO: This will probably be integrated into LevelData TBF 
{

    [SerializeField] LevelVisualDataSO defualtSO;
    Level_VisualData visualData;

    public Vector2 DamageRange => visualData.DamageRange;
    public Vector2 FontSizeRange => visualData.FontSizeRange;
    public float Crit_FontSizeBonus => visualData.Crit_FontSizeBonus;
    public AnimationCurve PopUpText_MoveCurve => visualData.PopUpText_MoveCurve;
    public AnimationCurve PopUpText_ScaleCurve => visualData.PopUpText_ScaleCurve;
    public AnimationCurve PopUpText_AlphaCurve => visualData.PopUpText_AlphaCurve;
    public float GetRelativeFontSizeForDamage(float damage) => Mathf.Clamp( (FontSizeRange.y - FontSizeRange.x) * (damage - DamageRange.x) / (DamageRange.y - DamageRange.x) + FontSizeRange.x, FontSizeRange.x, FontSizeRange.y);

    public override void Awake()
    {
        //Instance = this;
        base.Awake();
        visualData = defualtSO.level_VisualData;
    }

    public void SetLevelVisualDataSO(LevelVisualDataSO levelVisualDataSO)
    {
        visualData = levelVisualDataSO.level_VisualData;
    }

  
}
[System.Serializable]
public struct Level_VisualData
{
    public Vector2 DamageRange;
    public Vector2 FontSizeRange;
    public float Crit_FontSizeBonus;

    public AnimationCurve PopUpText_MoveCurve;
    public AnimationCurve PopUpText_ScaleCurve;
    public AnimationCurve PopUpText_AlphaCurve;
}

