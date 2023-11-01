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
        private const string MODIFIER_KEY_CODE = "{Modifier}";
        
        [SerializeField] private Transform _textSpawnPoint;

        [SerializeField] private float _lineMagnitude;
        [SerializeField] private float _lineStep;
        private float _currentPointerPlace = 0f;

        public void SpawnPopUp(StatChangeData config)
        {
            PopUpTextConfig popUpTextConfig;
            
            popUpTextConfig = config.UsePopUpTextConfig ? config.PopUpTextConfig : PopUpTextManager.Instance.DefaultPopUpConfig;
            
            if(popUpTextConfig.DisablePopUp)
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

            switch (popUpTextConfig.PopUpTextType)
            {
                case PopUpTextType.ShowName:
                    popUpTextConfig.Text = config.StatEffectName;
                    break;
                case PopUpTextType.ShowDelta:
                    popUpTextConfig.Text = Mathf.Round(config.Delta).ToString(CultureInfo.CurrentCulture);
                    break;
                case PopUpTextType.ShowNewValue:
                    popUpTextConfig.Text = Mathf.Round(config.NewValue).ToString(CultureInfo.CurrentCulture);
                    break;
                case PopUpTextType.ShowText:
                    string text = RegularExpressionsTool.SetValueOnKeyWord(popUpTextConfig.Text,
                        new Dictionary<string, object>() {{MODIFIER_KEY_CODE, config.Delta} });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!popUpTextConfig.OverrideSize)
                popUpTextConfig.FontSize = PopUpTextManager.Instance.GetRelativeFontSizeForDamage(config.Delta);

            PopupText popupText = PoolManager.PopUpTextPool.GetObject();
           
            popupText.gameObject.SetActive(true);
            popupText.transform.SetParent(PopUpTextManager.Instance._popUpTextCanvas.transform);
            popupText.transform.position = _textSpawnPoint.position;
            popupText.Init(popUpTextConfig);
        }
    }
}