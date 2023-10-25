using System;
using System.Globalization;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Systems.StatusSystem;
using UnityEngine;
using Object = UnityEngine.Object;

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
        [SerializeField] private BasicPopupText _textBoxPrefab;
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

            popUpTextConfig.Text = popUpTextConfig.PopUpTextType switch
            {
                PopUpTextType.ShowDelta => Mathf.Round(config.Delta).ToString(CultureInfo.CurrentCulture),
                PopUpTextType.ShowNewValue => Mathf.Round(config.NewValue).ToString(CultureInfo.CurrentCulture),
                PopUpTextType.ShowName => config.StatEffectName,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!popUpTextConfig.OverrideSize)
                popUpTextConfig.FontSize = PopUpTextManager.Instance.GetRelativeFontSizeForDamage(config.Delta);
            
            BasicPopupText popupText =
                Object.Instantiate(_textBoxPrefab,_textSpawnPoint.position, Quaternion.identity,PopUpTextManager.Instance._popUpTextCanvas.transform);
            popupText.Init(popUpTextConfig);
        }
    }
}