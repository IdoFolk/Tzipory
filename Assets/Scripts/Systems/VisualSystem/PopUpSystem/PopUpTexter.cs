using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

/*
 * ^ - Starts with
 * $ - Ends with
 * [] - Range
 * () - Group
 * . - Single character once
 * + - one or more characters in a row
 * ? - optional preceding character match
 * \ - escape character
 * \n - New line
 * \d - Digit
 * \D - Non-digit
 * \s - White space
 * \S - non-white space
 * \w - alphanumeric/underscore character (word chars)
 * \W - non-word characters
 * {x,y} - Repeat low (x) to high (y) (no "y" means at least x, no ",y" means that many)
 * (x|y) - Alternative - x or y
 *
 * [^x] - Anything but x (where x is whatever character you want)
 */

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
        private const string MODIFIER_KEY_CODE = "Modifie";
        
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
                    string text = popUpTextConfig.Text;
                    
                    bool foundKeyWord = false;
                    int keyWordCount = 0;

                    for (int i = 0; i < text.Length; i++)
                    {
                        int keyWordStartIndex = 0;
                
                        if (text[i] == '{')
                        {
                            foundKeyWord = true;
                            keyWordCount = 0;
                            keyWordStartIndex = i;
                        }

                        if(foundKeyWord)
                        {
                            if (text[i] != MODIFIER_KEY_CODE[keyWordCount])
                                foundKeyWord = false;
                            keyWordCount++;

                            if (keyWordCount == MODIFIER_KEY_CODE.Length && foundKeyWord)
                            {
                                var keyWordEndIndex = i;
                                popUpTextConfig.Text = text.Substring(0, keyWordStartIndex) + "" + text.Substring(keyWordEndIndex, text.Length);
                            }
                        }
                    }
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
        
        
        IEnumerable<string> FindKeywords(string s)
        {
            var matches = Regex.Matches(s, "{(.*?)}");
            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value;
            }
        }

    }
}