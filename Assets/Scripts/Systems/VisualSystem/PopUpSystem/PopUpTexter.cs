using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.RegularExpressions;
using UnityEngine;

namespace Tzipory.Systems.VisualSystem.PopUpSystem
{
    public enum TextSpawnRepeatPatterns
    {
        PingPong,
        LoopRight,
        LoopLeft
    };

    [Serializable]
    public class PopUpTexter
    {
        [SerializeField] private Transform _textSpawnPoint;

        [SerializeField] private float _lineMagnitude;
        [SerializeField] private float _lineStep;
        private float _currentPointerPlace = 0f;

        public void SpawnPopUp(StatChangeData changeData)
        {
            var popUpTextConfig = changeData.UsePopUpTextConfig ? changeData.PopUpTextConfig : PopUpTextManager.Instance.DefaultPopUpConfig;

            if (popUpTextConfig.DisablePopUp)
                return;

            Vector3 value = _textSpawnPoint.localPosition;

            switch (popUpTextConfig.RepeatPattern)
            {
                case TextSpawnRepeatPatterns.PingPong:
                    value.x = Mathf.Sin(_currentPointerPlace) * _lineMagnitude;
                    _currentPointerPlace += _lineStep;
                    break;
                case TextSpawnRepeatPatterns.LoopRight:
                    if (_currentPointerPlace >= _lineMagnitude)
                        _currentPointerPlace = _lineMagnitude * -1f;
                    value.x = _currentPointerPlace;
                    _currentPointerPlace += _lineStep;
                    break;
                case TextSpawnRepeatPatterns.LoopLeft:
                    if (_currentPointerPlace <= _lineMagnitude * -1f)
                        _currentPointerPlace = _lineMagnitude;
                    value.x = _currentPointerPlace;
                    _currentPointerPlace -= _lineStep;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _textSpawnPoint.localPosition = value;

            if (popUpTextConfig.RoundNumbers)
            {
                switch (popUpTextConfig.PopUpTextType)
                {
                    case PopUpTextType.ShowName:
                        popUpTextConfig.Text = changeData.StatEffectName;
                        break;
                    case PopUpTextType.ShowDelta:
                        popUpTextConfig.Text = changeData.Delta < 0 ? Mathf.Round(changeData.Delta * -1).ToString(CultureInfo.CurrentCulture) : 
                            Mathf.Round(changeData.Delta).ToString(CultureInfo.CurrentCulture);
                        break;
                    case PopUpTextType.ShowNewValue:
                        popUpTextConfig.Text = Mathf.Round(changeData.NewValue).ToString(CultureInfo.CurrentCulture);
                        break;
                    case PopUpTextType.ShowText:

                        var keyWordData = new Dictionary<string, object>()
                        {
                            { PopUpTextConfig.DeltaKeyCode, Mathf.Round(changeData.Delta) },
                            { PopUpTextConfig.ModifierKeyCode, Mathf.Round(changeData.Modifier) },
                            { PopUpTextConfig.NameKeyCode, changeData.StatEffectName },
                            { PopUpTextConfig.NewValueKeyCode, Mathf.Round(changeData.NewValue) }
                        };

                        popUpTextConfig.Text = RegularExpressionsTool.SetValueOnKeyWord(popUpTextConfig.Text, keyWordData);
                        break;
                    case PopUpTextType.ShowModifier:
                        popUpTextConfig.Text = Mathf.Round(changeData.Modifier).ToString(CultureInfo.CurrentCulture);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (popUpTextConfig.PopUpTextType)
                {
                    case PopUpTextType.ShowName:
                        popUpTextConfig.Text = changeData.StatEffectName;
                        break;
                    case PopUpTextType.ShowDelta:
                        popUpTextConfig.Text = changeData.Delta < 0 ? (changeData.Delta * -1).ToString(CultureInfo.CurrentCulture) : 
                           changeData.Delta.ToString(CultureInfo.CurrentCulture);
                        break;
                    case PopUpTextType.ShowNewValue:
                        popUpTextConfig.Text = changeData.NewValue.ToString(CultureInfo.CurrentCulture);
                        break;
                    case PopUpTextType.ShowText:

                        var keyWordData = new Dictionary<string, object>()
                        {
                            { PopUpTextConfig.DeltaKeyCode, changeData.Delta },
                            { PopUpTextConfig.ModifierKeyCode, changeData.Modifier },
                            { PopUpTextConfig.NameKeyCode, changeData.StatEffectName },
                            { PopUpTextConfig.NewValueKeyCode, changeData.NewValue }
                        };

                        popUpTextConfig.Text = RegularExpressionsTool.SetValueOnKeyWord(popUpTextConfig.Text, keyWordData);
                        break;
                    case PopUpTextType.ShowModifier:
                        popUpTextConfig.Text = changeData.Modifier.ToString(CultureInfo.CurrentCulture);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            if (!popUpTextConfig.OverrideSize)
                popUpTextConfig.FontSize = PopUpTextManager.Instance.GetRelativeFontSizeForDamage(changeData.Delta);

            PopupText popupText = PoolManager.PopUpTextPool.GetObject();

            popupText.gameObject.SetActive(true);
            popupText.transform.SetParent(PopUpTextManager.Instance._popUpTextCanvas.transform);
            popupText.transform.position = _textSpawnPoint.position;

            popupText.Init(popUpTextConfig);
        }
    }
}